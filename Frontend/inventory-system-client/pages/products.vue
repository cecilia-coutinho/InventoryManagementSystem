<template>
    <NavBar />
    <v-container>
        <v-row>
            <PageHeader :title="pageTitle" :text="pageDescription" />
        </v-row>
    </v-container>

    <DataFetcher :uri="apiBaseUri">
        <template #default="{ data, create, edit, deleteItem, refresh }">
            <TableComponent :title="pageTitle" :entityName="entityName" :headers="headers" :data="data"
                :entityKeys="entityKeys" :fkInfo="fkInfo" :create="create" :edit="edit" :deleteItem="deleteItem"
                :refresh="refresh" :openAddDialog="openAddDialog" :openEditDialog="openEditDialog"
                :openDetailsDialog="openDetailsDialog" :deleteEntity="deleteEntity" />
            <v-dialog v-model="dialog" persistent max-width="600px">
                <v-card>
                    <v-card-title>
                        <span class="headline">{{ dialogTitle }}</span>
                    </v-card-title>
                    <v-card-text>
                        <v-container>
                            <v-row>
                                <v-col cols="12" v-for="(field, index) in entityFields" :key="index">
                                    <v-text-field v-model="editedEntity[field.key]" :label="field.label"
                                        :rules="field.rules" :placeholder="field.placeholder"></v-text-field>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                    <v-card-actions>
                        <v-btn color="blue darken-1" text @click="close">Fechar</v-btn>
                        <v-btn color="#4caf50" text @click="() => save(create, edit, refresh)"
                            v-if="!isDetailsDialog">Salvar</v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
        </template>
    </DataFetcher>
    <PageFooter />
</template>

<script setup>
import { ref } from 'vue';

const pageTitle = 'Produtos';
const pageDescription = 'Administre seus produtos aqui.';
const entityName = 'Produto';

const headers = [
    'Nome do Produto',
    'Descrição',
    'Preço de Venda',
    'Preço de Compra',
    'Categoria'
];

const entityKeys = [
    'productName',
    'productDescription',
    'sellPrice',
    'costPrice',
    'fkProductCategory'
];

const fkInfo = {
    fkProductCategory: {
        uri: 'https://localhost:7171/api/ProductCategories',
        displayField: 'productCategoryName'
    }
};

const entityFields = [
    { key: 'productName', label: 'Nome do Produto', rules: [v => !!v || 'Nome do Produto é obrigatório', v => (v && v.length <= 100) || 'Nome do Produto deve ter no máximo 100 caracteres'] },
    { key: 'productDescription', label: 'Descrição do Produto' },
    { key: 'sellPrice', label: 'Preço de Venda', rules: [v => !!v || 'Preço de Venda é obrigatório', v => (v && !isNaN(v)) || 'Preço de Venda deve ser um número e use ponto (.) para separar os centavos'], placeholder: 'Use ponto (.) para separar os centavos' },
    { key: 'costPrice', label: 'Preço de Compra', rules: [v => !!v || 'Preço de Compra é obrigatório', v => (v && !isNaN(v)) || 'Preço de Compra deve ser um número e use ponto (.) para separar os centavos'], placeholder: 'Use ponto (.) para separar os centavos' },
    { key: 'fkProductCategory', label: 'Categoria do Produto', rules: [v => !!v || 'Categoria do Produto é obrigatório'] }
];

const dialog = ref(false);
const dialogTitle = ref('');
const isDetailsDialog = ref(false);
const currentEntityId = ref(null);

const editedEntity = ref({
    productName: '',
    productDescription: '',
    sellPrice: 0,
    costPrice: 0,
    fkProductCategory: ''
});

const apiBaseUri = 'https://localhost:7171/api/Products';

const openAddDialog = () => {
    dialog.value = true;
    dialogTitle.value = `Adicionar Novo ${entityName}`;
    editedEntity.value = {
        productName: '',
        productDescription: '',
        sellPrice: 0,
        costPrice: 0,
        fkProductCategory: ''
    };
};

const openEditDialog = (id, entity) => {
    dialogTitle.value = `Editar ${entityName}`;
    editedEntity.value = { ...entity };
    currentEntityId.value = id;
    dialog.value = true;
};

const openDetailsDialog = (id, entity) => {
    dialogTitle.value = `Detalhes do ${entityName}`;
    editedEntity.value = { ...entity };
    currentEntityId.value = id;
    dialog.value = true;
    isDetailsDialog.value = true;
};

const deleteEntity = async (id, deleteItem, refresh) => {
    await deleteItem(id);
    refresh();
};

const close = () => {
    dialog.value = false;
    isDetailsDialog.value = false;
};

const save = async (createFunction, editFunction, refresh) => {
    if (currentEntityId.value) {
        await editFunction(currentEntityId.value, editedEntity.value);
    } else {
        await createFunction(editedEntity.value);
    }
    dialog.value = false;
    close();
    refresh();
};
</script>

<style scoped>
.table-list {
    width: 100%;
    border-collapse: collapse;
}

.table-list th,
.table-list td {
    border: 1px solid #ddd;
    padding: 8px;
}

.table-list th {
    background-color: #f2f2f2;
    text-align: left;
}

.add-btn {
    display: flex;
    justify-content: flex-end;
    margin: 1rem 0;
}
</style>

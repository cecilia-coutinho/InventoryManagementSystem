<template>
    <NavBar />

    <v-container>
        <v-row>
            <PageHeader title="Produtos" text="Administre seus produtos aqui." />
        </v-row>
    </v-container>

    <DataFetcher :uri="apiBaseUri">
        <template #default="{ data, create, edit, deleteItem, refresh }">
            <div class="list-title">
                <h1 class="col-span-6 font-bold">Produtos</h1>
            </div>
            <table class="table-list">
                <thead>
                    <tr>
                        <th v-for="header in tableHeaders" :key="header" style="text-align: center;">
                            {{ header }}
                        </th>
                        <th style="width: 150px; text-align: center;">Ações</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="p in data.result" :key="p.id">
                        <td>{{ p.productName }}</td>
                        <td>{{ p.productDescription }}</td>
                        <td>{{ p.sellPrice }}</td>
                        <td>{{ p.costPrice }}</td>
                        <td>
                            <DataFetcher :uri="`${apiCategoryUri}/${p.fkProductCategory}`">
                                <template #default="{ data: categoryData }">
                                    {{ categoryData.productCategoryName }}

                                </template>
                            </DataFetcher>
                        </td>
                        <td>
                            <v-icon color="blue darken-1" small class="mr-2" style="margin: 8px;"
                                @click="openEditProductDialog(p.id, p, edit, refresh)">mdi-pencil</v-icon>
                            <v-icon color="#4caf50" small class="mr-2" style="margin: 8px;"
                                @click="openProductDetailsDialog(p.id, p)">mdi-magnify</v-icon>
                            <v-icon color="#ff5252" small class="mr-2" style="margin: 8px;"
                                @click="deleteProduct(p.id, deleteItem, refresh)">mdi-delete</v-icon>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="add-btn">
                <v-row class="add-btn">
                    <v-btn color="#4caf50" @click="openAddProductDialog"
                        style="margin-bottom: 2rem; margin-left: 1rem;">
                        <v-icon>mdi-plus</v-icon> Adicionar Produto
                    </v-btn>
                </v-row>
            </div>
            <v-dialog v-model="dialog" persistent max-width="600px">
                <v-card>
                    <v-card-title>
                        <span class="headline">{{ dialogTitle }}</span>
                    </v-card-title>
                    <v-card-text>
                        <v-container>
                            <v-row>
                                <v-col cols="12">
                                    <v-text-field v-model="editedProduct.productName" :rules="productNameRules"
                                        label="Nome do Produto"></v-text-field>
                                </v-col>
                                <v-col cols="12">
                                    <v-text-field v-model="editedProduct.productDescription"
                                        label="Descrição do Produto"></v-text-field>
                                </v-col>
                                <v-col cols="12">
                                    <v-text-field v-model="editedProduct.sellPrice" :rules="sellPriceRules"
                                        label="Preço de Venda" placeholder="Use ponto (.) para separar os centavos">
                                    </v-text-field>
                                </v-col>
                                <v-col cols="12">
                                    <v-text-field v-model="editedProduct.costPrice" :rules="costPriceRules"
                                        label="Preço de Compra" placeholder="Use ponto (.) para separar os centavos">
                                    </v-text-field>
                                </v-col>
                                <v-col cols="12">
                                    <v-text-field v-model="editedProduct.fkProductCategory"
                                        :rules="productCategoryRules" label="Categoria do Produto"></v-text-field>
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

const tableHeaders = [
    'Producto',
    'Descrição',
    'Preço de Venda',
    'Preço de Compra',
    'Categoria',
];

const productNameRules = computed(() => [
    v => !!v || 'Nome do Produto é obrigatório',
    v => (v && v.length <= 100) || 'Nome do Produto deve ter no máximo 100 caracteres'
]);

const sellPriceRules = computed(() => [
    v => !!v || 'Preço de Venda é obrigatório',
    v => (v && !isNaN(v)) || 'Preço de Venda deve ser um número e use ponto (.) para separar os centavos'
]);

const costPriceRules = computed(() => [
    v => !!v || 'Preço de Compra é obrigatório',
    v => (v && !isNaN(v)) || 'Preço de Compra deve ser um número e use ponto (.) para separar os centavos'
]);

const productCategoryRules = computed(() => [
    v => !!v || 'Categoria do Produto é obrigatório'
]);

const dialog = ref(false);
const dialogTitle = ref('');
const isDetailsDialog = ref(false);
const currentProductId = ref(null);

const editedProduct = ref({
    productName: '',
    productDescription: '',
    sellPrice: 0,
    costPrice: 0,
    productCategory: '',
});

const apiBaseUri = 'https://localhost:7171/api/Products';
const apiCategoryUri = 'https://localhost:7171/api/ProductCategories';

const openAddProductDialog = () => {
    dialog.value = true;
    dialogTitle.value = 'Adicionar Novo Produto';
    editedProduct.value = {
        productName: '',
        productDescription: '',
        sellPrice: 0,
        costPrice: 0,
        fkProductCategory: '',
    };
};

const openEditProductDialog = (id, product) => {
    dialogTitle.value = 'Editar Produto';
    editedProduct.value = { ...product };
    currentProductId.value = id;
    dialog.value = true;
};

const openProductDetailsDialog = (id, product) => {
    dialogTitle.value = 'Detalhes do Produto';
    editedProduct.value = { ...product };
    currentProductId.value = id;
    dialog.value = true;
    isDetailsDialog.value = true;
};

const deleteProduct = async (id, deleteItem, refresh) => {
    await deleteItem(id);
    refresh();
};

const close = () => {
    dialog.value = false;
    isDetailsDialog.value = false;
};

const save = async (createFunction, editFunction, refresh) => {
    if (currentProductId.value) {
        await editFunction(currentProductId.value, editedProduct.value);
    } else {
        await createFunction(editedProduct.value);
    }
    dialog.value = false;
    close();
    refresh();
};
</script>

<style scoped>
table {
    border-collapse: collapse;
    width: 100%;
}

th,
td {
    border: 1px solid #4caf50;
    padding: 8px;
    text-align: left;
}

th {
    background-color: #16161d;
    color: #fec859;
    font-weight: 200;
}

.mdi-icon {
    color: #4caf50;
}

.col-span-1 {
    font-size: 14px;
    padding: .2rem 1rem;
}

.list-title {
    display: flex;
    justify-content: center;
    align-items: center;
    margin: 1rem;
}

.add-btn {
    display: flex;
    justify-content: center;
    align-items: center;
    margin-top: 1.5rem;
}

.table-list {
    max-width: 90%;
    margin: 0 auto;
}
</style>

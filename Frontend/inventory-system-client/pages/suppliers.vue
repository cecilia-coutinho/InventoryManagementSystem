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
                :entityKeys="entityKeys" :create="create" :edit="edit" :deleteItem="deleteItem" :refresh="refresh"
                :openAddDialog="openAddDialog" :openEditDialog="openEditDialog" :openDetailsDialog="openDetailsDialog"
                :deleteEntity="deleteEntity" />
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

const pageTitle = 'Fornecedores';
const pageDescription = 'Administre seus fornecedores aqui.';
const entityName = 'Fornecedor';

const headers = [
    'Nome do Fornecedor',
    'Endereço',
    'Contato',
    'Email do Contato'
];

const entityKeys = [
    'supplierName',
    'supplierAddress',
    'contactName',
    'contactEmail'
];

const apiBaseUri = 'https://localhost:7171/api/Suppliers';

const entityFields = [
    { key: 'supplierName', label: 'Nome do Fornecedor', rules: [v => !!v || 'Nome do Fornecedor é obrigatório', v => (v && v.length <= 100) || 'Nome do Fornecedor deve ter no máximo 100 caracteres'] },
    { key: 'supplierAddress', label: 'Endereço' },
    { key: 'fkContactId', label: 'Id de Contato', rules: [v => !!v || 'Id de Contato é obrigatório'] }
];

const dialog = ref(false);
const dialogTitle = ref('');
const isDetailsDialog = ref(false);
const currentEntityId = ref(null);

const editedEntity = ref({
    supplierName: '',
    supplierAddress: '',
    fKContactId: null
});

const openAddDialog = () => {
    dialog.value = true;
    dialogTitle.value = `Adicionar Novo ${entityName}`;
    editedEntity.value = {
        supplierName: '',
        supplierAddress: '',
        fKContactId: null
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

.custom-select {
    background-color: #2A2A2A;
    color: #B0B0B0;
    display: block;
    width: 100%;
    padding: 0.375rem 0.75rem;
    font-size: 1rem;
    line-height: 2.5;
    border: 1px solid #2A2A2A;
    border-radius: 0.25rem;
    appearance: none;
    background-image: url('data:image/svg+xml;charset=UTF-8,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 4 5"><path fill="%23B0B0B0" d="M2 0L0 2h4z"/></svg>');
    background-repeat: no-repeat;
    background-position: right 0.75rem center;
    background-size: 0.65em auto;
}

.field-container {
    display: flex;
    flex-direction: column;
    margin-bottom: 1rem;
}
</style>

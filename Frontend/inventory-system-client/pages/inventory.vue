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
                                    <v-autocomplete v-if="field.key === 'fkProductId'"
                                                    v-model="selectedProduct"
                                                    :items="productOptions.productNames"
                                                    item-value="id"
                                                    item-text="productName"
                                                    :label="field.label"
                                                    :rules="field.rules"
                                                    :placeholder="field.placeholder"
                                                    :loading="isLoading"
                                                    no-data-text="Nenhum dado disponível"
                                                    @change="updateProductId"></v-autocomplete>
                                    <v-text-field v-else
                                                  v-model="editedEntity[field.key]"
                                                  :label="field.label"
                                                  :rules="field.rules"
                                                  :placeholder="field.placeholder"></v-text-field>
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
    const pageTitle = 'Inventário';
    const pageDescription = 'Administre seu inventário aqui.';
    const entityName = 'Inventário';

    const headers = [
        'Produto',
        'Quantidade em Estoque',
        'Estoque Mínimo',
        'Estoque Máximo'
    ];

    const entityKeys = [
        'fkProductId',
        'quantityInStock',
        'minStockLevel',
        'maxStockLevel'
    ];

    const fkInfo = {
        fkProductId: {
            uri: 'https://localhost:7171/api/Products',
            displayField: 'productName'
        }
    };

    const entityFields = [
        { key: 'fkProductId', label: 'Produto', rules: [v => !!v || 'Produto é obrigatório', v => (v && v.length <= 100) || 'Nome do Produto deve ter no máximo 100 caracteres'] },
        { key: 'quantityInStock', label: 'Quantidade em Estoque', rules: [v => !!v || 'Quantidade em Estoque é obrigatório', v => (v && !isNaN(v)) || 'Quantidade em Estoque deve ser um número', v => (v && v > 0) || 'Quantidade em Estoque deve ser um número positivo'] },
        { key: 'minStockLevel', label: 'Estoque Mínimo', rules: [v => !!v || 'Estoque Mínimo é obrigatório', v => (v && !isNaN(v)) || 'Estoque Mínimo deve ser um número', v => (v && v > 0) || 'Estoque Mínimo deve ser um número positivo'] },
        { key: 'maxStockLevel', label: 'Estoque Máximo', rules: [v => !!v || 'Estoque Máximo é obrigatório', v => (v && !isNaN(v)) || 'Estoque Máximo deve ser um número', v => (v && v > 0) || 'Estoque Máximo deve ser um número positivo'] }
    ];

    const dialog = ref(false);
    const dialogTitle = ref('');
    const isDetailsDialog = ref(false);
    const currentEntityId = ref(null);

    const editedEntity = ref({
        fkProductId: '',
        quantityInStock: 0,
        minStockLevel: 0,
        maxStockLevel: 0
    });

    const apiBaseUri = 'https://localhost:7171/api/Inventory';

    const productOptions = ref({
        productNames: [],
        dictionary: {}
    });
    const isLoading = ref(true);
    const selectedProduct = ref('');
    const handleError = (error) => {
        console.error('Failed to fetch data:', error);
    };

    const fetchProductOptions = async () => {
        try {
            const { uri, displayField } = fkInfo.fkProductId;
            const { data, error } = await useFetch(uri);
            if (data.value && data.value.result && Array.isArray(data.value.result)) {
                productOptions.value.dictionary = {};
                productOptions.value.productNames = data.value.result.map(item => {
                    productOptions.value.dictionary[item.id] = item[displayField];
                    return item[displayField];
                });
            }
            else if (error.value) {
                handleError();
            }
            else {
                console.error('Products data is not an array or is empty:', data.value);
            }
        } catch (error) {
            handleError();
        } finally {
            isLoading.value = false;
        }
    };

    const updateProductId = () => {
        const selectedProductId = Object.keys(productOptions.value.dictionary).find(
            id => productOptions.value.dictionary[id] === selectedProduct.value
        );
        editedEntity.value.fkProductId = selectedProductId || '';
    };

    watch(selectedProduct, updateProductId);
    onMounted(fetchProductOptions);

    const openAddDialog = () => {
        dialog.value = true;
        dialogTitle.value = `Adicionar Novo ${entityName}`;
        editedEntity.value = {
            fkProductId: '',
            quantityInStock: 0,
            minStockLevel: 0,
            maxStockLevel: 0
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

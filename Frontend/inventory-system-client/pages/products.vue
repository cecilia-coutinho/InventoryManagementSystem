<template>
    <NavBar />

    <v-container>
        <v-row>
            <PageHeader title="Produtos" text="Administre seus produtos aqui." />
        </v-row>
    </v-container>

    <DataFetcher :uri="apiBaseUri">
        <template #default="{ data, create, edit, deleteItem }">
            <div class="list-title">
                <h1 class="col-span-6 font-bold">Produtos</h1>
            </div>
            <div class="grid grid-cols-6 gap-4">
                <div v-for="p in data.result" :key="p.id" style="border: 1px solid #4caf50; ">
                    <div class="product-icon">
                        <v-icon class="product-icon">mdi-cart-outline</v-icon>
                    </div>
                    <div class="col-span-1">{{ p.productName }}</div>
                    <div class="col-span-1">{{ p.productDescription }}</div>
                    <div class="col-span-1">Preco de venda: {{ p.sellPrice }}</div>
                    <div class="col-span-1">Preco de custo: {{ p.costPrice }}</div>
                    <div class="col-span-1">Categoria: {{ p.fkProductCategory }}</div>
                    <div class="col-span-1 d-flex">
                        <v-icon small class="mr-2" @click="openEditProductDialog(p.id, p, edit)">mdi-pencil</v-icon>
                        <v-icon small class="mr-2" @click="openProductDetailsDialog(p.id, p)">mdi-magnify</v-icon>
                        <v-icon small class="mr-2" @click="deleteProduct(p.id, deleteItem)">mdi-delete</v-icon>
                    </div>
                </div>
            </div>
            <div class="add-btn">
                <v-row class="add-btn">
                    <v-btn color="#4caf50" @click="openAddProductDialog" style="margin-bottom: 2rem; margin-left: 1rem;">
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
                                    <v-text-field v-model="editedProduct.productName" label="Nome do Produto"></v-text-field>
                                </v-col>
                                <v-col cols="12">
                                    <v-text-field v-model="editedProduct.productDescription" label="Descrição do Produto"></v-text-field>
                                </v-col>
                                <v-col cols="12">
                                    <v-text-field v-model="editedProduct.sellPrice" label="Preço de Venda"></v-text-field>
                                </v-col>
                                <v-col cols="12">
                                    <v-text-field v-model="editedProduct.costPrice" label="Preço de Compra"></v-text-field>
                                </v-col>
                                <v-col cols="12">
                                    <v-text-field v-model="editedProduct.fkProductCategory" label="Categoria do Produto"></v-text-field>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                    <v-card-actions>
                        <v-btn color="blue darken-1" text @click="close">Fechar</v-btn>
                        <v-btn color="#4caf50" text @click="() => save(create, edit)">Salvar</v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
        </template>

    </DataFetcher>
    <PageFooter />
</template>

<script setup>
    import { ref } from 'vue';

    const dialog = ref(false);
    const dialogTitle = ref('');
    const currentProductId = ref(null);

    const editedProduct = ref({
        productName: '',
        productDescription: '',
        sellPrice: 0,
        costPrice: 0,
        productCategory: '',
    });

    const apiBaseUri = 'https://localhost:7171/api/Products/';

    const headers = [
        { text: 'ID', value: 'id' },
        { text: 'Nome', value: 'productName' },
        { text: 'Descrição', value: 'productDescription' },
        { decimal: 'Preço de Venda', value: 'sellPrice' },
        { decimal: 'Preço de Compra', value: 'costPrice' },
        { guid: 'Categoria', value: 'fkProductCategory' },
    ];

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
    };

    const deleteProduct = async (id, deleteItem) => {
        await deleteItem(id);
    };

    const close = () => {
        dialog.value = false;
    };

    const save = async (createFunction, editFunction) => {
        if (currentProductId.value) {
            await editFunction(currentProductId.value, editedProduct.value);
        } else {
            await createFunction(editedProduct.value);
        }
        close();
    };
</script>

<style scoped>
    .grid {
        display: flex;
        flex-wrap: wrap;
        max-width: 80%;
        margin: 0 auto;
    }

        .grid > div {
            flex: 1 0 200px;
            margin: 1rem;
            border: 1px solid #4caf50;
            border-radius: 5px;
            box-sizing: border-box;
        }

    .mdi-icon {
        color: #4caf50;
    }

    .product-icon {
        color: #fec859;
        display: flex;
        justify-content: center;
        align-items: center;
        margin: 1rem;
        font-size: 3rem;
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
</style>



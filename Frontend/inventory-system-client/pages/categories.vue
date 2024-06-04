<template>
    <NavBar />

    <v-container>
        <v-row>
            <PageHeader title="Categorias" text="Administre suas categorias aqui." />
        </v-row>

        <v-row>
            <v-btn color="#4caf50" @click="openAddCategoryDialog" style="margin-bottom: 2rem; margin-left: 1rem;">
                <v-icon>mdi-plus</v-icon> Adicionar Categoria
            </v-btn>
        </v-row>

        <DataFetcher :uri="apiBaseUri">
            <template #default="{ data, create, edit}">
                <div class="grid grid-cols-2">
                    <v-card-title>Categorias</v-card-title>
                    <v-card v-for="c in data" :key="c.id" class="category-card">
                        <v-card-text>
                            {{ c.productCategoryName }}
                            <v-icon small class="mr-2" @click="openEditCategoryDialog(c.id, c, edit)">mdi-pencil</v-icon>
                        </v-card-text>
                    </v-card>
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
                                        <v-text-field v-model="editedCategory.productCategoryName" label="Category Name"></v-text-field>
                                    </v-col>
                                </v-row>
                            </v-container>
                        </v-card-text>
                        <v-card-actions>
                            <v-btn color="blue darken-1" text @click="close">Close</v-btn>
                            <v-btn color="#4caf50" text @click="() => save(create, edit, refresh)">Save</v-btn>
                        </v-card-actions>
                    </v-card>
                </v-dialog>
            </template>
        </DataFetcher>
    </v-container>

    <PageFooter />
</template>

<style scoped>
    .category-card {
        border: 1px solid #fec859;
        background-color: transparent;
        margin-top: 1.5rem;
    }
</style>

<script setup>
    import { ref } from 'vue';

    const dialog = ref(false);
    const dialogTitle = ref('');
    const editedCategory = ref({ productCategoryName: '' });
    const currentCategoryId = ref(null);
    const apiBaseUri = ref('https://localhost:7171/api/ProductCategories');

    function openAddCategoryDialog() {
        dialogTitle.value = 'Adicionar Nova Categoria';
        editedCategory.value = { productCategoryName: '' };
        currentCategoryId.value = null;
        dialog.value = true;
    }

    function openEditCategoryDialog(id, category) {
        dialogTitle.value = 'Editar Categoria';
        editedCategory.value = { ...category };
        currentCategoryId.value = id;
        dialog.value = true;
    }

    function close() {
        dialog.value = false;
    }

    async function save(createFunction, editFunction) {
        if (currentCategoryId.value) {
            await editFunction(currentCategoryId.value, editedCategory.value);
        } else {
            await createFunction(editedCategory.value);
        }
        close();
    }
</script>
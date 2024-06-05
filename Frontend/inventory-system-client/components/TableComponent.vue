<template>
    <div>
        <div class="list-title">
            <h1 class="col-span-6 font-bold">{{ title }}</h1>
        </div>
    </div>

    <v-data-table class="table-list" :items="data.result" :items-per-page="-1">
        <thead>
            <tr>
                <th v-for="header in headers" :key="header" style="text-align: center;">
                    {{ header }}
                </th>
                <th style="width: 150px; text-align: center;">Ações</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="entity in data.result" :key="entity.id">
                <td v-for="key in entityKeys" :key="key">
                    <template v-if="fkInfo[key]">
                        <DataFetcher :uri="`${fkInfo[key].uri}/${entity[key]}`">
                            <template #default="{ data: fkData }">
                                {{ fkData[fkInfo[key].displayField] }}
                            </template>
                        </DataFetcher>
                    </template>
                    <template v-else>
                        {{ entity[key] }}
                    </template>
                </td>
                <td>
                    <v-icon color="blue darken-1" small class="mr-2" style="margin: 8px;"
                            @click="openEditDialog(entity.id, entity, edit, refresh)">mdi-pencil</v-icon>
                    <v-icon color="#4caf50" small class="mr-2" style="margin: 8px;"
                            @click="openDetailsDialog(entity.id, entity)">mdi-magnify</v-icon>
                    <v-icon color="#ff5252" small class="mr-2" style="margin: 8px;"
                            @click="deleteEntity(entity.id, deleteItem, refresh)">mdi-delete</v-icon>
                </td>
            </tr>
        </tbody>
        <template v-slot:bottom>
        </template>
    </v-data-table>
    <div class="add-btn">
        <v-row class="add-btn">
            <v-btn color="#4caf50" @click="openAddDialog" style="margin-bottom: 2rem; margin-left: 1rem;">
                <v-icon>mdi-plus</v-icon> Adicionar {{ entityName }}
            </v-btn>
        </v-row>
    </div>
</template>

<script setup>
const props = defineProps({
    title: {
        type: String,
        required: true
    },
    entityName: {
        type: String,
        required: true
    },
    headers: {
        type: Array,
        required: true
    },
    data: {
        type: Object,
        required: true
    },
    entityKeys: {
        type: Array,
        required: true
    },
    fkInfo: {
        type: Object,
        required: false,
        default: () => ({})
    },
    create: Function,
    edit: Function,
    deleteItem: Function,
    refresh: Function,
    openAddDialog: Function,
    openEditDialog: Function,
    openDetailsDialog: Function,
    deleteEntity: Function
});

const __options__ = {
    name: 'TableComponent',
}
</script>

<style scoped>
table {
    border-collapse: collapse;
    width: 100%;
}
.table-list {
    max-width: 90%;
    margin: 0 auto;
}

.table-list th,
.table-list td {
    border: 1px solid #4caf50;
        padding: 8px;
        text-align: left;
}

.table-list th {
    background-color: #16161d;
    color: #fec859;
    font-weight: 200;
    text-align: left;
}

.list-title {
    display: flex;
    justify-content: center;
    align-items: center;
    margin: 1rem;
}

.col-span-1 {
    font-size: 14px;
    padding: .2rem 1rem;
}

.add-btn {
    display: flex;
    justify-content: center;
    align-items: center;
    margin-top: 1.5rem;
}
</style>

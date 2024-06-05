<template>
    <div>
        <slot :data="data" :create="createItem" :edit="editItem" :deleteItem="deleteItem" :refresh="fetchData"></slot>
    </div>
</template>

<script setup>
    import { ref, onMounted, watch } from 'vue';

    defineOptions({
        name: 'DataFetcher'
    });

    const props = defineProps({
        uri: {
            type: String,
            required: true
        }
    });

    const data = ref([]);

    async function fetchData() {
        try {
            const { data: fetchedData, error } = await useFetch(props.uri);
            if (error.value) {
                console.error('Error fetching data:', error.value);
            } else {
                data.value = fetchedData.value;
            }
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    }

    async function createItem(item) {
        try {
            const response = await $fetch(props.uri, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(item)
            });
            data.value = response;
        } catch (error) {
            console.error('Error creating item:', error);
        }
    }

    async function editItem(id, item) {
        try {
            await $fetch(`${props.uri}/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(item)
            });
            if (data.value.id === id) {
                for (let key in item) {
                    data.value[key] = item[key];
                }
            }
        } catch (error) {
            console.error('Error editing item:', error);
        }
    }

    async function deleteItem(id) {
        try {
            await $fetch(`${props.uri}/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                }
            });
            data.value = data.value.filter(d => d.id !== id);
        } catch (error) {
            console.error('Error deleting item:', error);
        }
    }

    onMounted(fetchData);
    watch(() => props.uri, fetchData);
</script>

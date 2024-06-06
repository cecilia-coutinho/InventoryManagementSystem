<template>
    <v-navigation-drawer v-model="drawer" app class="custom-nav-drawer">
        <v-list nav>
            <v-list-item :to="{ path: '/' }" prepend-icon="mdi-view-dashboard" title="Dashboard" />
            <v-list-item :to="{ path: '/categories' }" prepend-icon="mdi-folder-outline" title="Categorias" />
            <v-list-item :to="{ path: '/products' }" prepend-icon="mdi-cart-outline" title="Productos" />
            <v-list-item :to="{ path: '/inventory' }" prepend-icon="mdi-package-variant-closed" title="Inventário" />
            <v-list-item :to="{ path: '/suppliers' }" prepend-icon="mdi-truck-outline" title="Fornecedores" />
        </v-list>
    </v-navigation-drawer>

    <v-app-bar app style="background-color: #16161d; color: #bb86fc">
        <v-app-bar-nav-icon @click="toggleDrawer"></v-app-bar-nav-icon>
        <v-toolbar-title title="Sistema de Inventário">Sistema de Inventário</v-toolbar-title>
    </v-app-bar>
</template>

<script>
    export default {
        name: 'NavBar',
        data() {
            return {
                drawer: false,
            };
        },
        methods: {
            toggleDrawer() {
                this.drawer = !this.drawer;
            },
        },
    };
</script>

<style scoped>
    .custom-nav-drawer {
        background-color: #16161d;
        color: #bb86fc;
        padding: 20px;
    }
</style>

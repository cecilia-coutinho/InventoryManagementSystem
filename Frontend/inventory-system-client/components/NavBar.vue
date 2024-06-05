<template>
    <v-app-bar app style="background-color: #16161d; color: #bb86fc">
        <v-toolbar-title>Sistema de Inventário</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn text :to="{ path: '/' }">Home</v-btn>
        <v-btn text :to="{ path: '/categories' }">Categorias</v-btn>
        <v-btn text :to="{ path: '/products' }">Productos</v-btn>
        <v-btn text :to="{ path: '/inventory' }">Inventário</v-btn>
        <v-btn text :to="{ path: '/suppliers' }">Fornecedores</v-btn>
    </v-app-bar>
</template>

<script>
export default {
    name: 'NavBar',
}
</script>

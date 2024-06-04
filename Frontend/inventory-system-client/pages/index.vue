<template>
    <v-app>
        <NavBar />

        <v-main>
            <v-container>
                <v-row>
                    <PageHeader title="Painel de Controle" text="Bem-vindo ao Sistema de Gerenciamento de Inventário" />

                    <DashboardCard title="Categorias" :to="{ path: '/categories' }" icon="mdi-folder-outline"
                        color="#fec859" iconColor="#fec859" />
                    <DashboardCard title="Produtos" :to="{ path: '/products' }" icon="mdi-cart-outline" color="#43b5a0"
                        iconColor="#43b5a0" />
                    <DashboardCard title="Fornecedores" :to="{ path: '/suppliers' }" icon="mdi-truck-outline"
                        color="#491d88" iconColor="#491d88" />
                </v-row>
            </v-container>
        </v-main>

        <PageFooter />
    </v-app>
</template>

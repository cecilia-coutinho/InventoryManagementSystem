<template>
    <v-app dark>
        <v-main>
            <v-container fluid fill-height>
                <v-row align="center" justify="center">
                    <v-col cols="12" sm="10" md="6">
                        <v-card class="elevation-12">
                            <v-toolbar color="error" dark flat>
                                <v-icon large left class="alert-icon">mdi-alert-circle</v-icon>
                                <v-toolbar-title>Página de Erro</v-toolbar-title>
                            </v-toolbar>
                            <v-card-text>
                                <v-card-title class="card-title" v-if="error.statusCode === 404">
                                    <v-icon large left>mdi-emoticon-sad-outline</v-icon>
                                    {{ pageNotFound }}
                                </v-card-title>
                                <v-card-title class="card-title" v-else>
                                    <v-icon large left>mdi-emoticon-confused-outline</v-icon>
                                    {{ otherError }}
                                </v-card-title>
                            </v-card-text>
                            <v-card-actions>
                                <v-spacer></v-spacer>
                                <NuxtLink to="/">
                                    <v-btn color="accent">
                                        <v-icon left>mdi-home</v-icon>
                                        Página inicial
                                    </v-btn>
                                </NuxtLink>
                            </v-card-actions>
                        </v-card>
                    </v-col>
                </v-row>
            </v-container>
        </v-main>
    </v-app>
</template>

<script>
    export default {
        name: 'EmptyLayout',
        layout: 'empty',
        props: {
            error: {
                type: Object,
                default: null
            }
        },
        data() {
            return {
                pageNotFound: '404 Não encontrado',
                otherError: 'Ocorreu um erro'
            }
        },
        head() {
            const title =
                this.error.statusCode === 404 ? this.pageNotFound : this.otherError
            return {
                title
            }
        }
    }
</script>

<style scoped>
    .card-title {
        font-size: 24px;
        text-align: center;
    }

    .v-toolbar {
        color: white;
    }

                .toolbar-title {
                    padding-left: 3rem;
                    margin-top: auto;   
                }

    .alert-icon {
        padding-left: 3rem;
    }
</style>

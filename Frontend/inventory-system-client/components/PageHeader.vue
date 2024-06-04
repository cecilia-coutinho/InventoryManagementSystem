<template>
    <v-col cols="12" class="pa-4">
        <v-card class="mx-auto page-header-card outlined-card">
            <v-container fluid>
                <v-row dense>
                    <v-col>
                        <v-card-title class="text-h6 text-md-h5 text-lg-h4">{{ title }}</v-card-title>
                        <v-card-text class="text-body-1">{{ text }}</v-card-text>
                    </v-col>
                </v-row>
            </v-container>
        </v-card>
    </v-col>
</template>

<script>
export default {
    name: 'PageHeader',
    props: {
        title: String,
        text: String,
    },
}
</script>

<style scoped>
.page-header-card {
    max-width: 100%;
    margin-top: 2rem;
    margin-bottom: 1.5rem;
}
.outlined-card {
    border: 1px solid #E77D67;
}
</style>

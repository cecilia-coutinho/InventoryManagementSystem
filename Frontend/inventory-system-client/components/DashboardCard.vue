<template>
    <v-col cols="12" sm="6" md="4">
        <router-link :to="to" class="no-underline no-color">
            <v-card class="bordered-card pa-4" :style="{ borderColor: color }" variant="outlined">
                <v-card-title class="d-flex flex-column align-center justify-center">
                    <v-icon :color="iconColor" :icon="icon" size="x-large">{{ icon }}</v-icon>
                    <div>{{ title }}</div>
                </v-card-title>
                <v-card-actions class="justify-center">
                    <v-btn text>Acessar</v-btn>
                </v-card-actions>
            </v-card>
        </router-link>
    </v-col>
</template>

<script>
export default {
    name: 'DashboardCard',
    props: {
        title: String,
        to: Object,
        icon: String,
        color: String,
        iconColor: String,
    },
}
</script>

<style scoped>
.bordered-card {
    border-width: 1px;
    border-style: solid;
}

.no-underline {
    text-decoration: none;
}

.no-color {
    color: inherit;
}

.v-icon {
    font-size: 4rem;
}
</style>

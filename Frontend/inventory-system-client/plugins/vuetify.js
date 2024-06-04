// import this after install `@mdi/font` package
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import { md2 } from 'vuetify/blueprints'


export default defineNuxtPlugin((nuxtApp) => {
    const vuetify = createVuetify({
        ssr: true,
        blueprint: md2,
        theme: {
            defaultTheme: 'dark',
            themes: {
                light: {
                    colors: {
                        primary: '#331a38',
                        secondary: '#fec859',
                        accent: '#ff5252',
                        error: '#fa448c',
                        info: '#491d88',
                        success: '#4caf50',
                        warning: '#e6155a'
                    }
                },
            },
        },
    })
    nuxtApp.vueApp.use(vuetify)
})

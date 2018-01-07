import Vue from 'vue'
import Buefy from 'buefy'
import Croppa from 'vue-croppa'
import 'vue-croppa/dist/vue-croppa.css'
import bus from './Bus.js'
import 'buefy/lib/buefy.css'
import 'font-awesome/css/font-awesome.min.css'
import axios from './axios.js'
import VueRouter from 'vue-router'
import xApplication from './Components/xApplication.vue'
import xLogin from './Components/xLogin.vue'
import xRegister from './Components/xRegister.vue'
import xSettings from './Components/xSettings.vue'
import xDashboard from './Components/xDashboard.vue'

const routes = [
    { path: '/login', component: xLogin },
    { path: '/register', component: xRegister },
    { path: '/settings', component: xSettings },
    { path: '/dashboard', component: xDashboard }
];

const router = new VueRouter({ routes });

Vue.use(Croppa)
Vue.use(axios)
Vue.use(VueRouter)
Vue.use(bus)
Vue.use(Buefy, {
	 defaultIconPack: 'fa'
})

Vue.component('x-login', xLogin)
Vue.component('x-register', xRegister)
Vue.component('x-dashboard', xDashboard)

window.xApplication = new Vue({
  el: '#x',
  router,
  render: h => h(xApplication)
})

window.Vue = Vue;
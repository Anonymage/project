import Vue from 'vue'
import axios from './axios.js'

Vue.use(axios)

const bus = new Vue({
    data: function(){
        return {
        	LoginState: 'loggedout',
        	Context: {
        		IMGURL: ''
        	}
        }
    },
    created(){

    	if(localStorage.getItem('Context') != null){
    		this.Context = localStorage.getItem('Context');
    	}

    	if(localStorage.getItem('Access-Token') != null){
    		this.LoginState = 'loggedin';
    	}

    	this.$axios.interceptors.request.use(function (config) {

    		if(localStorage.getItem('Access-Token') != null){
    			config.headers["Authorization"] = 'Bearer ' + localStorage.getItem('Access-Token');
    		}
    		return config;
  		}.bind(this), function (error) {
  			return Promise.reject(error);
  		}.bind(this));

		this.$axios.interceptors.response.use(function (response) {
			return response;
  		}.bind(this), function (error) {
  			for (var key in error.response.data) {
  				this.$emit('xNotifyError', '(' + key + ') ' + error.response.data[key]);
  			}
    		return Promise.reject(error);
  		}.bind(this));
    },
    beforeDestroy(){
    	localStorage.setItem('Context', this.Context);
    }
});

bus.install = function (Vue, options) {
    Vue.prototype.$bus = bus
}

export default bus
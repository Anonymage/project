<template>
	<div class="card x-small-card">
	  <header class="card-header">
	    <p class="card-header-title">Register</p>
	  </header>
	  <div class="card-content">
	    <div class="content">
	    	<p>Please enter a Username, Email (optional) and Password.</p>
	      <section>
	        <b-field>
	            <b-input icon="user" v-model="RegisterModel.USERNAME" placeholder="Enter a Username"></b-input>
	        </b-field>
	        <b-field>
	            <b-input icon="envelope" v-model="RegisterModel.MAIL" placeholder="Enter an E-Mail"></b-input>
	        </b-field>
	        <b-field>
	            <b-input icon="unlock-alt" type="password" v-model="RegisterModel.PASSWORD" placeholder="Enter your password" password-reveal></b-input>
	        </b-field>
	       </section>
	    </div>
	  </div>
	  <footer class="card-footer">
	  	<a href="#/login" class="card-footer-item">Back to Login</a>
	    <a href="#" class="card-footer-item" @click="Register">Register</a>
	  </footer>
	</div>
</template>

<script>

	export default {
		name: 'x-login',
		data: function(){
			return {
				RegisterModel: {
					USERNAME: '',
					PASSWORD: '',
					MAIL: ''
				}
			}
		},
		methods: {
			Register: function(){
				this.$axios.post('api/account/register', this.RegisterModel)
					.then(function(r){
						localStorage.setItem('Access-Token', r.data);
						this.$bus.LoginState = 'loggedin'
						this.$bus.$emit('xNotifySuccess', 'New User ' + this.RegisterModel.USERNAME + ' is logged in!');
						this.$router.push({ path: '/dashboard' });
					}.bind(this));
			}
		}
	}

</script>
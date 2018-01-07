<template>
	<div class="card x-small-card">
	  <header class="card-header">
	    <p class="card-header-title">Login</p>
	  </header>
	  <div class="card-content">
	    <div class="content">
	    	<p>Please Login or click Register to create a new account.</p>
	      <section>
	        <b-field>
	            <b-input icon="user" v-model="LoginModel.USERNAME" placeholder="Enter your Username"></b-input>
	        </b-field>
	        <b-field>
	            <b-input icon="unlock-alt" type="password" v-model="LoginModel.PASSWORD" placeholder="Enter your password" password-reveal>
	            </b-input>
	        </b-field>
	       </section>
	    </div>
	  </div>
	  <footer class="card-footer">
	    <a href="#/register" class="card-footer-item">Register</a>
	    <a href="#" class="card-footer-item" @click="Login">Login</a>
	  </footer>
	</div>
</template>

<script>

	export default {
		name: 'x-login',
		data: function(){
			return {
				LoginModel: {
					USERNAME: '',
					PASSWORD: ''
				}
			}
		},
		methods:{
			Login: function(){
				this.$axios.post('/token', this.LoginModel)
					.then(function(r){
						localStorage.setItem('Access-Token', r.data);
						this.$bus.LoginState = 'loggedin';
						this.$bus.$emit('xNotifySuccess', this.LoginModel.USERNAME + ' is logged in!');
						this.$router.push({ path: '/dashboard' });
					}.bind(this));
			}
		}
	}

</script>
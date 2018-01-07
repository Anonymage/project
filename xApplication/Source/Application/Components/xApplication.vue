<template>
	<section class="home">
		<header class="navbar header has-shadow is-primary">
			<div class="container">
				<div class="navbar-brand">
				<a class="navbar-item" href="#/dashboard">X-Application</a>
				</div>
				<div class="navbar-menu is-active" v-show="this.$bus.LoginState == 'loggedin'">
					<div class="navbar-end">
						<div class="navbar-item has-dropdown is-hoverable">
							<div class="navbar-link">Settings</div>
							<div class="navbar-dropdown is-boxed">
								<a class="navbar-item" href="#/dashboard">Dashboard</a>
								<a class="navbar-item" href="#/settings">Account-Settings</a>
								<a class="navbar-item" href="#/login" @click="Logout">Logout</a>
							</div>
						</div>
					</div>
				</div>
			</div>
		</header>
		<div class="hero is-fullheight is-primary">
			<div class="hero-body">
				<div class="container has-text-centered">
					<router-view></router-view>
				</div>
			</div>
		</div>
		<footer class="footer">
			<div class="container">
				<div class="content">
					<p>Volatile</p>
				</div>
			</div>
		</footer>
	</section>
</template>

<script>

	export default {
		name: 'x-application',
		data () {
			return {
				
			}  
		},
		created(){
			this.$bus.$on('xNotifyError', function(message){
				this.$toast.open({
                    duration: 3000,
                    message: message,
                    position: 'is-bottom',
                    type: 'is-danger'
                });
      		}.bind(this));

			this.$bus.$on('xNotifySuccess', function(message){
				this.$toast.open({
                    message: message,
                    type: 'is-success'
                });
			}.bind(this));
		},
		methods:{
			Logout: function(){
				localStorage.removeItem('Access-Token');
				this.$bus.LoginState = 'loggedout';
				this.$bus.$emit('xNotifySuccess', 'You have been logged out!');
			}
		}
	}

</script>

<style>
	.x-small-card {
		max-width:500px;
		margin:0 auto;
		margin-top: -100px;
	}
</style>
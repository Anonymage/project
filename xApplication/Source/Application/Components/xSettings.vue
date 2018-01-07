<template>
	<div class="card">
	  <header class="card-header">
	    <p class="card-header-title">Account-Settings</p>
	  </header>
	  <div class="card-content">
	    <div class="content">
	      <section>
	      	<croppa class="x-image-uploader" v-model="croppa" placeholder="Upload an Image">
	      		<img :src="SettingsModel.IMGURL" slot="initial">
	      	</croppa>
	      	<br/>
	      	<a class="button is-primary x-save-img-btn" @click="SaveImageChanges">Save Image Changes</a>
	        <b-field>
	            <b-input icon="user" v-model="SettingsModel.USERNAME" placeholder="Enter a Username"></b-input>
	        </b-field>
	        <b-field>
	            <b-input icon="envelope" v-model="SettingsModel.MAIL" placeholder="Enter an E-Mail"></b-input>
	        </b-field>
	        <b-field>
	            <b-input icon="unlock" type="password-old" v-model="SettingsModel.PASSWORD" placeholder="Enter your old password" password-reveal></b-input>
	        </b-field>
	        <b-field>
	            <b-input icon="unlock-alt" type="password-new" v-model="SettingsModel.NEWPASSWORD" placeholder="Enter your new password" password-reveal></b-input>
	        </b-field>
	        <b-field>
	            <b-input icon="check" type="password-confirm" v-model="SettingsModel.CONFIRMPASSWORD" placeholder="Confirm your password" password-reveal></b-input>
	        </b-field>
	       </section>
	    </div>
	  </div>
	  <footer class="card-footer">
	    <a href="#/dashboard" class="card-footer-item">Go Back to Dashboard</a>
	    <a href="#" class="card-footer-item" @click="Save">Save</a>
	  </footer>
	</div>
</template>

<script>

	export default {
		name: 'x-login',
		data: function(){
			return {
				croppa: {},
				SettingsModel: {
					USERNAME: '',
					MAIl: '',
					NEWPASSWORD: '',
					PASSWORD: '',
					CONFIRMPASSWORD: '',
					IMGURL: ''
				}
			}
		},
		created(){
			this.$axios.get('api/settings')
				.then(function(r){
					this.SettingsModel.USERNAME = r.data.username;
					this.SettingsModel.MAIL = r.data.mail;
					this.SettingsModel.IMGURL = r.data.imgurl;
				}.bind(this));
		},
		methods:{
			Save: function(){
				this.$axios.post('api/settings', this.SettingsModel)
					.then(function(r){
						this.$bus.$emit('xNotifySuccess', 'Your settings have been updated!');
					}.bind(this));
			},
			SaveImageChanges: function(){
				this.croppa.generateBlob((blob) => { 
					var reader = new FileReader();
					reader.readAsDataURL(blob); 
					reader.onloadend = function() {
					    this.$axios.post('api/settings/avatar', { 
					    	image: reader.result.substr(reader.result.indexOf(',') + 1, reader.result.length - 1)
					    })
					    .then(function(r){
					    	this.$bus.$emit('xNotifySuccess', 'Image-Changes have been saved!');
					    }.bind(this));
					}.bind(this);
				}, 'image/png', 1.0);
			},
			doxxxx: function(){
				console.log(this.croppa);
			}
		}
	}

</script>

<style>
	
	.x-image-uploader {
		margin-bottom:25px;
	}

	.x-save-img-btn {
		margin-bottom:20px;
	}

</style>
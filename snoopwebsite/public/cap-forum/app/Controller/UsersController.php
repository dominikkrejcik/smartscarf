<?php
App::uses('Controller', 'AppController');

class UsersController extends AppController {
	
	public function beforeFilter() {
		parent::beforeFilter();
		
		$this->Auth->allow('profile');
	}
	
	public function profile($id=null) {
		
	}
	
	public function login() {
		if($this->request->is('post')) {
			if ($this->Auth->login()) {
				$this->redirect($this->Auth->redirect());
			} else {
				$this->Session->setFlash(__('Invalid username or password'));
			}
		}
	}
	
	public function logout() {
		$this->redirect($this->Auth->logout());
	}
}

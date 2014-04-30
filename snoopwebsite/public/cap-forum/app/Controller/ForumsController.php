<?php
App::uses('AppController', 'Controller');

class ForumsController extends AppController {

	public $components = array('Paginator');
	
	public function beforeFilter() {
		$this->Auth->allow();
	}
	
	public function index() {
		$this->Paginator->settings['contain'] = array('Topic', 'Post'=>array('User','Topic'));
		$this->set('forums', $this->Paginator->paginate());
	}

}
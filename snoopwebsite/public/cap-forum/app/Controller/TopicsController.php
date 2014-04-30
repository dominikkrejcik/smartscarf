<?php
App::uses('AppController', 'Controller');

class TopicsController extends AppController {

	public $components = array('Paginator');
	
	public function beforeFilter() {
		$this->Auth->allow('index','view');
	}
	
	public function index($forumId=null) {
		if (!$this->Topic->Forum->exists($forumId)) {
			throw new NotFoundException(__('Invalid forum'));
		}
		
		$forum = $this->Topic->Forum->read(null,$forumId);
		$this->set('forum',$forum);
		
		$this->Paginator->settings['contain'] = array('User','Post'=>array('User'));
		$this->set('topics', $this->Paginator->paginate());
	}
	
	public function add() {
		$forums = $this->Topic->Forum->find('list');
		
		if ($this->request->is('post')) {
			$this->request->data['Topic']['user_id'] = $this->Auth->user('id');
			if ($this->Topic->save($this->request->data)) {
				$this->Session->setFlash(__('Topic has been created'));
				$this->redirect('/');
			}
		} 
		
		$this->set('forums',$forums);
	}

	public function view($id) {
		if (!$this->Topic->exists($id)) {
			throw new NotFoundException(__('Invalid topic'));
		}
		
		$topic = $this->Topic->read(null,$id);
		$forum = $this->Topic->Forum->read(null,$topic['Topic']['forum_id']);
		$this->Paginator->settings['Post']['conditions'] = array('Post.topic_id'=>$topic['Topic']['id']);
		$this->Paginator->settings['Post']['contain'] = array('User');
		$this->Paginator->settings['Post']['order'] = array('Post.id'=>'DESC');
		
		$this->set('topics', $this->Paginator->paginate('Post'));
		
		$this->set('topic', $topic);
		$this->set('forum', $forum);
		$this->set('posts', $this->Paginator->paginate('Post'));
	}
}
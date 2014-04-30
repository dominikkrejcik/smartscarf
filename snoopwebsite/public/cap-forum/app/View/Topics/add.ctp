<div class="row">
      	  <div class="col-lg-12">
	      	  	<ol class="breadcrumb">
				  <li>
				  	<?php echo $this->Html->link(__('Forum'),'/')?>
				  </li>
				  <li class="active">
				   	<?php echo __('Create Topic');?>
				  </li>
				</ol>
      	  </div>
</div>

<div class="row">  
      	  <div class="col-lg-12">
      	  	<p class="lead"><?php echo __('Create Topic');?></p>
      	  	
      	  	<div class="well">
	      	  	<?php echo $this->Form->create('Topic',array('class'=>'form-horizontal','inputDefaults'=>array('label'=>false)));?>
				  <div class="form-group">
				    <label for="inputEmail3" class="col-sm-2 control-label">Title</label>
				    <div class="col-sm-10">
				       <?php echo $this->Form->input('name',array('class'=>'form-control'));?>
				    </div>
				  </div>
				  <div class="form-group">
				    <label for="inputPassword3" class="col-sm-2 control-label">Forum</label>
				    <div class="col-sm-10">
						      <?php echo $this->Form->input('forum_id',array('options'=>$forums, 'class'=>'form-control'));?>
				    </div>
				  </div>
				 <div class="form-group">
				    <label for="inputPassword3" class="col-sm-2 control-label">Content</label>
				    <div class="col-sm-10">
				     	<?php echo $this->Form->textarea('content',array('class'=>'form-control','rows'=>5));?>
				    </div>
				  </div>
				  
				  <div class="form-group">
				    <div class="col-sm-offset-2 col-sm-10">
				      <?php echo $this->Form->submit(__('Create'),array('class'=>'btn btn-primary'))?>
				    </div>
				  </div>
				</form>
			</div>
	      	  	
	      </div>
 </div> 
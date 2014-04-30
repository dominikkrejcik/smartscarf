<div class="row">
	<div class="col-lg-12">
		<ol class="breadcrumb">
			<li>
				<?php echo $this->Html->link(__('Forum'),'/')?>
			</li>
			<li>
				<?php echo $this->Html->link($forum['Forum']['name'],array('controller'=>'topics','action'=>'index',$forum['Forum']['id']))?>
			</li>
			<li class="active">
				<?php echo $topic['Topic']['name'];?>
			</li>
		</ol>
	</div>
</div>
<div class="row">
	<div class="col-lg-8">
		<p class="lead">
		<?php echo $topic['Topic']['content'];?>
		</p>
	</div>

	<div class="col-lg-4">
		<p class="text-right">
			<?php echo $this->Html->link(__('Create Topic'),array('action'=>'add'),
															array('class'=>'btn btn-primary'))?>

			<?php echo $this->Html->link(__('Post Reply'),array('controller'=>'posts','action'=>'add',$topic['Topic']['id']),
															array('class'=>'btn btn-primary'))?>
		</p>
	</div>
</div>
<div class="row">
	<div class="col-lg-4">
		<p style="font-weight: bold;">
		<?php 
	      	  	echo $this->Paginator->counter(
	      	  			'Showing {:start} - {:end} of {:count}'
	      	  	);
	      	  	?>
		</p>
	</div>
</div>

<div class="row">
	<div class="col-lg-12">
		<table class="table table-bordered  ">


			<tbody>
				<?php 
				foreach ($posts as $post) :
				?>
					<tr>
						<td><small>
							<?php 
								echo $this->Time->timeAgoInWords($post['Post']['created']);
							?></small>
						</td>
						<td>&nbsp;</td>
					</tr>
					</tr>
					<td width="150px">
						<p>
							<?php
								echo $this->Html->link($post['User']['username'],
														array('controller'=>'users','action'=>'profile',$post['User']['id']));
							?>
						</p> 
						<?php $hash = md5($post['User']['email']);?>
						<img src="http://www.biography.com/imported/images/Biography/Images/Profiles/S/Snoop-Dogg-197052-1-402.jpg" >
					</td>
					<td>
						<p>
							<?php echo $post['Post']['content'];?>
						</p>
					</td>
					</tr>
				<?php endforeach;?>
			</tbody>

		</table>
		<div class="pull-right">
			<?php 
				echo $this->element('paginator');
		    ?>
		</div>
		<div class="clearfix"></div>
		<div class="well">
			<h4><?php echo __('Quick Reply');?></h4>
			<?php echo $this->Form->create('Post',array('url'=>array('controller'=>'posts','action'=>'add'),
														 'inputDefaults'=>array('label'=>false)));?>
				<div class="form-group">
					<?php echo $this->Form->textarea('content',array('class'=>'form-control','rows'=>5));?>
				</div>
				<?php echo $this->Form->hidden('topic_id',array('value'=>$topic['Topic']['id']));?>
				<?php echo $this->Form->hidden('forum_id',array('value'=>$forum['Forum']['id']));?>
				<?php echo $this->Form->submit(__('Post Reply'),array('class'=>'btn btn-primary'))?>
			<?php echo $this->Form->end();?>
		</div>
	</div>
</div>

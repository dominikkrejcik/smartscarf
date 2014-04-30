<div class="row">
      	  <div class="col-lg-12">
	      	  	<ol class="breadcrumb">
				  <li>
				  	<?php echo $this->Html->link(__('Forum'),'/')?>
				  </li>
				  <li class="active">
				   <?php echo $forum['Forum']['name']?>
				  </li>
				</ol>
      	  </div>
</div>

<div class="row">  	  
      	  <div class="col-lg-4">
	      	  <p style="font-weight:bold;">
	      	  	<?php 
	      	  	echo $this->Paginator->counter(
	      	  			'Showing {:start} - {:end} of {:count}'
	      	  	);
	      	  	?>
	      	  </p>
      	  </div>
      	  
      	  <div class="col-lg-8">
      	  	<p class="text-right">
      	  		<?php echo $this->Html->link(__('Create Topic'),array('action'=>'add'),array('class'=>'btn btn-primary'))?>
      	  	</p>
      	  </div>
</div>  
    
     
<div class="row">
      	  <div class="col-lg-12 ">
	      	  	<table class="table table-bordered">
					<thead>
						<tr>
							<th colspan=2>Topic</th>
							<th>Author</th>
							<th>Created</th>
							<th>Posts</th>
							<th>Activity</th>
						</tr>
					</thead>
					
					<tbody>
						<?php foreach ($topics as $topic): ?>
						<tr>
							<td>&nbsp;</td>
							<td>
								<?php 
								echo $this->Html->link($topic['Topic']['name'],
														array('controller'=>'topics','action'=>'view',$topic['Topic']['id']));
								?>
							</td>
							<td><?php
								echo $this->Html->link($topic['User']['username'],
														array('controller'=>'users','action'=>'profile',$topic['User']['id']));
								?>
							</td>
							<td><?php 
									echo $this->Time->timeAgoInWords($topic['Topic']['created']);
								?>
							</td>
							<td>
							   <?php 
							   	echo count($topic['Post']);
							   ?>
							</td>
							<td>
							 <?php 
							   if(count($topic['Post'])>0) { 
							   	$post = $topic['Post'][0];
							   	echo $this->Time->timeAgoInWords($post['created']);
							   	echo '&nbsp;<small>by</small>&nbsp;';
							   	echo $this->Html->link($post['User']['username'],array('controller'=>'users',
							   																	'action'=>'profile',
							   																	$post['User']['id']));
							   } 
							   ?>
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
	      </div>
</div>
 
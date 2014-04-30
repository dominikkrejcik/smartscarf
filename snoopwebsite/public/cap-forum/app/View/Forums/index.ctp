<div class="row">
      	  <div class="col-lg-12 ">
	      	  	<table class="table table-bordered">
					<thead>
						<tr>
							<th colspan=2>Forum</th>
							<th>Topics</th>
							<th>Posts</th>
							<th>Activity</th>
						</tr>
					</thead>
					
					<tbody>
						<?php foreach ($forums as $forum): ?>
						<tr>
							<td>&nbsp;</td>
							<td>
								<?php 
								echo $this->Html->link('<h4>'.$forum['Forum']['name'].'</h4>',
														array('controller'=>'topics','action'=>'index',$forum['Forum']['id']),
														array('escape'=>false));
								?>
							</td>
							<td><?php echo count($forum['Topic']);?></td>
							<td><?php echo count($forum['Post']);?></td>
							<td>
							   <?php 
							   if(count($forum['Post'])>0) {
							   	$post = $forum['Post'][0];
							   	echo $this->Html->link($post['Topic']['name'],array('controller'=>'topics',
																				   			'action'=>'view',
																				   			$post['Topic']['id']));
							   	echo '&nbsp;';
							   	echo $this->Time->timeAgoInWords($post['created']);
							   	echo '&nbsp;<small>by</small>&nbsp;';
							   	echo '&nbsp;';
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
 
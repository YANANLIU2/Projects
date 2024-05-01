.data
# player data
pac_man_player: .byte # mannually converted a pac-man player pixel art in a format of (x,y,x_length)
7,2,5,
5,3,8,
4,4,10,
4,5,10,
3,6,7,
3,7,6,
3,8,4,
3,9,6,
3,10,7,
4,11,10,
4,12,10,
5,13,8,
6,14,5
.align 2 

pac_man_player_length: .word 39
player_pos_x: .word 11
player_pos_y: .word 18
player_direction_x: .word 0
player_direction_y: .word 0

.text
.globl update_player_movement
#######################################################
# void update_player_movement()
# handles player movement updates and redraws the player if needed
update_player_movement:
    # intro 
    subi $sp, $sp, 48
    sw $ra, 40($sp)
    sw $s5, 36($sp)
    sw $s4, 32($sp)
    sw $s3, 28($sp)
    sw $s2, 24($sp)
    sw $s1, 20($sp)
    sw $s0, 16($sp)
    
    # load player direction
    lw $s0, player_direction_x
    lw $s1, player_direction_y     
    
    # return if player direction is (0,0)
    or $t0, $a0, $a1
    beq $t0, $zero, Lend_update_player_movement
    
    # desired pos
    lw $s4, player_pos_x
    add $s2, $s4, $s0  # t2: new_x
    lw $s5, player_pos_y
    add $s3, $s5, $s1  # t3: new_y
    
    # check for teleportation
    move $a0, $s2
    jal check_for_teleportation
    move $s2, $v0

    # calc the corresponding index on the map of the new player pos: index = new_y * map_width + new_x
    lw $t0, pac_man_map_width
    mul $t0, $s3, $t0
    add $t0, $t0, $s2
    
    # read the symbol on map at new player pos
    la $t1, pac_man_map
    add $t0, $t1, $t0 # addr + index
    lb $t0, ($t0)
    
    bne $t0, 0, Lend_update_player_movement

    # save
    sw $s2, player_pos_x
    sw $s3, player_pos_y

Ldraw_player:
    # overwrite the previous player position with a black square
    move $a0, $s4
    move $a1, $s5
    jal draw_a_black_square
    jal draw_player
    
Lend_update_player_movement:
     # outro
    lw $ra, 40($sp)
    lw $s5, 36($sp)
    lw $s4, 32($sp)
    lw $s3, 28($sp)
    lw $s2, 24($sp)
    lw $s1, 20($sp)
    lw $s0, 16($sp)
    addi $sp, $sp, 48
    jr $ra

.globl draw_player
#######################################################
# void draw_player()
# draw the player bitmap based on player position
draw_player:
    # intro 
    subi $sp, $sp, 56
    sw $ra, 52($sp)
    sw $s5, 40($sp)
    sw $s4, 36($sp)
    sw $s3, 32($sp)
    sw $s2, 28($sp)
    sw $s1, 24($sp)
    sw $s0, 20($sp)
    lw $t0, color_yellow
    sw $t0, 16($sp)
    
    # draw
    lw $a0, pac_man_map_start_x
    lw $t0, player_pos_x
    add $a0, $a0, $t0
    jal tile_unit_to_pixel_pos
    move $s3, $v0
    
    lw $a0, pac_man_map_start_y
    lw $t0, player_pos_y
    add $a0, $a0, $t0
    jal tile_unit_to_pixel_pos
    move $s4, $v0
    
    li $s2, 0 # cur i
    la $s1, pac_man_player
    lw $s0, pac_man_player_length
    
Ldraw_player_begin:
    # draw horizontal rectangles of (x, y, x_length, 1) using datasets from pac_man_player
    beq $s2, $s0,  Ldraw_player_end
    
    lb $a0, 0($s1)
    add $a0, $a0, $s3
    
    lb $a1, 1($s1)
    add $a1, $a1, $s4 
    
    lb $a2, 2($s1)
    li $a3, 1
    jal draw_rectangle
    
    addi $s2, $s2, 3
    addi $s1, $s1, 3
    b Ldraw_player_begin
    
Ldraw_player_end:
    # outro
    lw $ra, 52($sp)
    lw $s5, 40($sp)
    lw $s4, 36($sp)
    lw $s3, 32($sp)
    lw $s2, 28($sp)
    lw $s1, 24($sp)
    lw $s0, 20($sp)
    addi $sp, $sp, 56
    jr $ra
    


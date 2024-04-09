#
# Bitmap Display Configuration:
# - Unit width in pixels: 1					     
# - Unit height in pixels: 1
# - Display width in pixels: 512
# - Display height in pixels: 512
# - Base Address for Display: 0x10040000 ($gp)

# Instructions of the game
# press wasd to move 
# press q to quit

.data
# Base address for the bitmap display
base_address: .word 0x10040000
display_width_units: .word 512 # display width/ unit width in pixels
display_tile_scale: .word 4 # in the form of 2**n 
tile_size_pixels: .word 16 # one tile = 16 pixels

# pacman map data
pac_man_map: .byte # mannually converted simplified pac-man game map
1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1,
1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1,
1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1,
1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1,
1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1,
1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1,
1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1,
1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1,
0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1,
1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1,
1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1,
1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1,
1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1,
1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1,
1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1,
1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1,
1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1,
1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1,
1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
.align 2

pac_man_map_width: .word 23 # how many units the map's width contains
pac_man_map_height:.word 25 # how many units the map's height contains
pac_man_map_start_x: .word 4  # offset of the whole map'x
pac_man_map_start_y: .word 4  # offset of the whole map'y
color_blue: .word 0x000000FF
color_gray: .word 0x00EEEEEE
color_yellow: .word 0x00FFFF00
color_black: .word 0x00000000
teleportation_y: .word 11 # the teleportation door in the middle of the map. The player can use it to travel from one end to another end. 

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

# input 
receiver_control_reg: .word 0xffff0000
receiver_data_reg: .word 0xffff0004

.text
.globl main

#################### main() #########################
main:
    # intro 
    subi $sp, $sp, 24
    sw $ra, 20($sp)
    
    # initial rendering
    jal draw_pac_man_map
    jal draw_player
    # jal draw_background
    
    # main loop
Lmain_loop_begin:
    jal update_input 
    beq $v0, $zero, Lmain_loop_end
    
    jal update_player_movement
    
    # Pause execution for 100 milliseconds to control game speed/ reduce cpu usage/ synchronize game logic
    li $v0, 32
    li $a0, 100
    syscall
    
    b Lmain_loop_begin
Lmain_loop_end:  
  
    # outro
    lw $ra, 20($sp)
    addi $sp, $sp, 24
    jr $ra

#######################################################
# int update_input()
# return 0 to quit the game. return 1 as default
update_input:
    # check for input 
    lw $a0, receiver_control_reg
    lw $a0, 0($a0)
    andi $a0, $a0, 0x0001
    beq $a0, $zero, Lhandle_input_return_one
    
    # process input
    lw $a0, receiver_data_reg
    lw $a0, 0($a0)
    
    beq $a0, 'd', Lpressed_right
    beq $a0, 'a', Lpressed_left
    beq $a0, 'w', Lpressed_up
    beq $a0, 's', Lpressed_down
    beq $a0, 'q', Lpressed_q
    b Lhandle_input_return_one
    
Lpressed_right:
    li $a0, 1
    sw $a0, player_direction_x
    li $a1, 0
    sw $a1, player_direction_y
    b Lhandle_input_return_one
    
Lpressed_left:
    li $a0, -1
    sw $a0, player_direction_x
    li $a1, 0
    sw $a1, player_direction_y
    b Lhandle_input_return_one
    
Lpressed_up:
    li $a0, 0
    sw $a0, player_direction_x
    li $a1, -1
    sw $a1, player_direction_y
    b Lhandle_input_return_one
    
Lpressed_down:
    li $a0, 0
    sw $a0, player_direction_x
    li $a1, 1
    sw $a1, player_direction_y
    b Lhandle_input_return_one

Lpressed_q:
    li $v0, 0 # means to quit the game
    jr $ra

Lhandle_input_return_one:
    li $v0, 1
    jr $ra

#######################################################
# void update_player_movement()
# handles player movement updates and redraws the player if needed
update_player_movement:
    # intro 
    subi $sp, $sp, 24
    sw $ra, 20($sp)
    
    # load player direction
    lw $a0, player_direction_x
    lw $a1, player_direction_y     
    
    # return if player direction is (0,0)
    or $t0, $a0, $a1
    beq $t0, $zero, Lend_update_player_movement
    
    # desired pos
    lw $t0, player_pos_x
    add $t2, $t0, $a0  # t2: new_x
    lw $t1, player_pos_y
    add $t3, $t1, $a1  # t3: new_y
    
    # check collision step 1: calc the corresponding index on the map of the new player pos: index = new_y * map_width + new_x
    lw $t6, pac_man_map_width
    mul $t4, $t3, $t6
    add $t4, $t4, $t2
    
    # check collision step 2: read the symbol on map at new player pos
    la $t5, pac_man_map
    add $t5, $t5, $t4 # addr + index
    lb $t5, ($t5)
    
    # check collision step 3: check for special wall scenario
    beq $t5, 1, Lcheck_for_tunnel

Lsave_player_new_pos:
    # save
    sw $t2, player_pos_x
    sw $t3, player_pos_y

    # overwrite the previous player position with a black square
    move $a0, $t0
    move $a1, $t1
    jal draw_a_black_square
    jal draw_player
    
Lend_update_player_movement:
     # outro
    lw $ra, 20($sp)
    addi $sp, $sp, 24
    jr $ra

Lcheck_for_tunnel:
    # check collision step 4: check if new_y matches teleportation_y. if not => do not update player's pos cuz we hit a wall
    lw $t7, teleportation_y
    bne $t7, $t3, Lend_update_player_movement
    
    # check collision step 4-1: check if new_x is -1.If it is => tele to the rightest tile. If not => check if new_x is map_width
    seq $v0, $t2, -1
    bne $v0, 1, Lcheck_tele_player_to_leftest
    subi $t2, $t6, 1
    b Lsave_player_new_pos
    
Lcheck_tele_player_to_leftest:
    # check collision step 4-2: check if new_x is map_width. If it is => tele to the leftest tile. if not => do not update player's pos cuz we hit a wall
    seq $v0, $t2, $t6 
    bne $v0, 1, Lend_update_player_movement
    li $t2, 0
    b Lsave_player_new_pos

#######################################################
# void draw_a_black_square(int pos_x, int pos_y)
# drarw a 1*1 black square at the designated pos
draw_a_black_square:
    # intro 
    subi $sp, $sp, 32
    sw $ra, 28($sp)
    sw $s0, 24($sp)
    lw $t0, color_black
    sw $t0, 16($sp)	

    # draw
    lw $t0, pac_man_map_start_x
    add $a0, $a0, $t0
    jal tile_unit_to_pixel_pos
    move $s0, $v0 

    lw $t0, pac_man_map_start_y
    add $a0, $a1, $t0
    jal tile_unit_to_pixel_pos
    move $a1, $v0 
    
    move $a0, $s0
    lw $a2, tile_size_pixels
    lw $a3, tile_size_pixels
    
    jal draw_rectangle
    	
    # outro
    lw $ra, 28($sp)
    lw $s0, 24($sp)
    addi $sp, $sp, 32
    jr $ra

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

#######################################################
# void draw_background()
# draw two rectangles
draw_background:
    # intro 
    subi $sp, $sp, 24
    sw $ra, 20($sp)
    lw $t0, color_gray  
    sw $t0, 16($sp)
   
    # draw rectangle 1
    li $a0, 0
    li $a1, 0
    lw $a2, pac_man_map_start_x
    lw $a3, display_width_units
    
    jal draw_rectangle
	
    # draw rectangle 2
    lw $a0, pac_man_map_start_x
    lw $t0, pac_man_map_width
    add $a0, $a0, $t0
    li $a1, 0
    lw $a3, display_width_units
    sub $a2, $a3, $a0
    jal draw_rectangle
	
    # outro
    lw $ra, 20($sp)
    addi $sp, $sp, 24
    jr $ra

#######################################################
# void draw_pac_man_map()
# draw the pacman maze
draw_pac_man_map:
    # intro 
    subi $sp, $sp, 56
    sw $ra, 52($sp)  
    sw $s7, 48($sp)	
    sw $s6, 44($sp)
    sw $s5, 40($sp)
    sw $s4, 36($sp)
    sw $s3, 32($sp)
    sw $s2, 28($sp)
    sw $s1, 24($sp)
    sw $s0, 20($sp)
    lw $t0, color_blue
    sw $t0, 16($sp)
	
    # draw
    lw $s0, pac_man_map_width
    lw $s1, pac_man_map_height
    la $s2, pac_man_map
    li $s3, 0 # index_x
    li $s4, 0 # index_y
    lw $s5, pac_man_map_start_x
    lw $s6, pac_man_map_start_y
	
    # for each '1' symbol, draw a 1*1 blue square
Lloop_begin_y:
    bge $s4, $s1, Lloop_end_y
	
    # start index_x at 0
    li $s3, 0
		
Lloop_begin_x:
    bge $s3, $s0, Lloop_increment_y
	
    # calc char index = y * map_witdth + x + addr offset
    mul $t4, $s4, $s0
    add $t4, $t4, $s3
    add $t4, $t4, $s2
    lb $t0, ($t4)
    bne $t0, 1, Lloop_increment_x
	
    # calc x = index_x + start_x
    add $a0, $s3, $s5
    jal tile_unit_to_pixel_pos
    move $s7, $v0

    # calc y = index_y + start_y
    add $a0, $s4, $s6
    jal tile_unit_to_pixel_pos
    move $a1, $v0

    move $a0, $s7
    lw $a2, tile_size_pixels
    lw $a3, tile_size_pixels
    jal draw_rectangle
    
Lloop_increment_x:
    addi $s3, $s3, 1
    b Lloop_begin_x

Lloop_increment_y:
    addi $s4, $s4, 1
    b Lloop_begin_y
	
Lloop_end_y:	
    # outro
    lw $ra, 52($sp)  
    lw $s7, 48($sp)	
    lw $s6, 44($sp)
    lw $s5, 40($sp)
    lw $s4, 36($sp)
    lw $s3, 32($sp)
    lw $s2, 28($sp)
    lw $s1, 24($sp)
    lw $s0, 20($sp)
    addi $sp, $sp, 56
    jr $ra

#######################################################
# int tile_unit_to_pixel_pos(int unit_pos)
# translate a tile pos to a pixel pos
tile_unit_to_pixel_pos:
    lw $t0, display_tile_scale
    sllv $v0, $a0, $t0
    jr $ra 

#######################################################
# void draw_rectangle(x, y, width, height, color)
# draw a width*height rectangle with the leftmost corner at (x,y)
draw_rectangle: 
    lw $v0, base_address
    lw $t0, display_width_units
    lw $v1, 16($sp)
    add $t6, $a1, $a3 # y_max
    
row_loop:
    # while(y < y_max)
    bge $a1, $t6, end_draw
    
    move $t5, $a0 # x. set to x start pos when begin to draw a new row
    add $t7, $a0, $a2 # x_max
col_loop:
    # while(x < x_max)
    bge $t5, $t7, next_row 
    
    # Calculate address of the pixel: (y * display_width + x) * 4 pixels + base address
    mul $t8, $a1, $t0
    add $t8, $t8, $t5
    sll $t8, $t8, 2
    add $t8, $v0, $t8
    
    # Write color to memory (bitmap display)
    sw $v1, 0($t8) 
    
    addi $t5, $t5, 1
    b col_loop
    
next_row:
    addi $a1, $a1, 1
    b row_loop

end_draw:
    jr $ra
    

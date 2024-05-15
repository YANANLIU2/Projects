.include "Pacman_player.asm"
.include "Pacman_ghosts.asm"
.include "Pacman_utils.asm"

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
pac_man_map_max_distance: .word 1060 # (23-1)^2 + (25-1)^2

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
    jal draw_all_ghosts
    #jal draw_background
    
    # seeding by the current time
    li $v0, 30
    syscall
    move $a0, $v0
    li $v0, 40
    syscall

    # main loop
Lmain_loop_begin:
    jal update_input 
    beq $v0, $zero, Lmain_loop_end
    
    jal update_player_movement
    jal update_all_ghosts
    jal draw_all_ghosts
    # Pause execution for 100 milliseconds to control game speed/ reduce cpu usage/ synchronize game logic
    li $v0, 32
    li $a0, 150
    syscall
    
    b Lmain_loop_begin
Lmain_loop_end:  
  
    # outro
    lw $ra, 20($sp)
    addi $sp, $sp, 24
    jr $ra

#######################################################
# bool update_input()
# return false to quit the game
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
    li $v0, 0 # quit game
    jr $ra

Lhandle_input_return_one:
    li $v0, 1
    jr $ra

#######################################################
# void draw_background()
# draw two rectangles
draw_background:
    # intro 
    subi $sp, $sp, 40
    sw $ra, 36($sp)
    sw $s1, 24($sp)
    sw $s0, 20($sp)
    lw $t0, color_gray  
    sw $t0, 16($sp)
   
    # draw rectangle 1
    lw $a0, pac_man_map_start_x
    jal tile_unit_to_pixel_pos
    move $s0, $v0
   
    lw $a3, display_width_units
    move $a2, $s0
    li $a0, 0
    li $a1, 0
    jal draw_rectangle
   
    # draw rectangle 2
    lw $a0, pac_man_map_start_x
    lw $t0, pac_man_map_width
    add $a0, $a0, $t0
    jal tile_unit_to_pixel_pos
    move $a0, $v0
    
    lw $a3, display_width_units
    li $a1, 0
    sub $a2, $a3, $a0
    jal draw_rectangle
	
    # outro
    lw $ra, 36($sp)
    sw $s1, 24($sp)
    lw $s0, 20($sp)
    addi $sp, $sp, 40
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
    
    

	

    

.data
# ghost data
ghost_bitmap: .byte # mannually converted a pac-man player pixel art in a format of (x,y,width,length)
6,1,3,1,
4,2,7,1,
3,3,9,1,
2,4,11,3,
1,7,13,6,
1,13,2,1,
4,13,2,1,
8,13,3,1,
12,13,2,1
1,14,1,1,
5,14,1,1,
8,14,2,1,
13,14,1,1
.align 2 

ghost_bitmap_length: .word 52

blinky_pos_x: .word 15
blinky_pos_y: .word 10

pinky_pos_x: .word 11
pinky_pos_y: .word 11

inky_pos_x: .word 10
inky_pos_y: .word 11

clyde_pos_x: .word 12
clyde_pos_y: .word 11

error_msg_out_of_range: .asciiz "Erros: Random number is out of range.\n"
.align 2
ghost_available_moves: .space 32 # Position array[4]. Position includes pos_x and pos_y
ghost_available_moves_num: .word 0
.text

.globl draw_all_ghosts
#######################################################
draw_all_ghosts:
    subi $sp, $sp, 24
    sw $ra, 20($sp)
    
    # draw "Blinky (red)" 
    lw $a0, blinky_pos_x
    lw $a1, blinky_pos_y
    lw $a2, color_red
    jal draw_ghost
    
    # draw "Pinky (pink)"
    lw $a0, pinky_pos_x
    lw $a1, pinky_pos_y
    lw $a2, color_pink
    jal draw_ghost
    
    # draw "Inky (cyan)"
    lw $a0, inky_pos_x
    lw $a1, inky_pos_y
    lw $a2, color_cyan
    jal draw_ghost
    
    # draw "Clyde (orange)"
    lw $a0, clyde_pos_x
    lw $a1, clyde_pos_y
    lw $a2, color_orange
    jal draw_ghost
    
    # outro
    lw $ra, 20($sp)
    addi $sp, $sp, 24
    jr $ra
    
.globl update_all_ghosts
#######################################################
# void update_all_ghosts()
# handles all ghosts' movement updates
update_all_ghosts:
    # intro 
    subi $sp, $sp, 24
    sw $ra, 20($sp)

    # update "Blinky (red)"
    la $a0, blinky_pos_x
    la $a1, blinky_pos_y
    jal update_ghost_move_towards_player
    
    # update "Pinky (pink)"
    la $a0, pinky_pos_x
    la $a1, pinky_pos_y
    jal update_ghost_move_towards_player
    
    # update "Inky (cyan)"
    la $a0, inky_pos_x
    la $a1, inky_pos_y
    jal update_ghost_move_towards_player
    
    # update "Clyde (orange)"
    la $a0, clyde_pos_x
    la $a1, clyde_pos_y
    jal update_ghost_move_towards_player
    
    # outro
    lw $ra, 20($sp)
    addi $sp, $sp, 24
    jr $ra
    
#######################################################
# int get_move_towards_player()
# return min distance index represents the best move among ghost_available_moves
#
# I also want to share an interesting finding. When I was trying to implement AI chase logic, 
# I initially wanted to use the Manhattan distance (abs(x) + abs(y)), but I realized that I 
# would need to write a branch for it. So instead, I used x^2 + y^2.
get_move_towards_player:
    # intro 
    subi $sp, $sp, 32
    sw $ra, 28($sp)
    sw $s0, 24($sp)
    sw $s1, 20($sp)
    lw $s0, player_pos_x
    lw $s1, player_pos_y 
    
    # choose the best move depends on an estimated distance from a move towards the player's pos
    li $v0, 0 # min distance index
    la $t0, ghost_available_moves
    lw $t1, ghost_available_moves_num
    lw $t2, pac_man_map_max_distance # min distance
    li $t3, 0 # cur i
    
loop_closest_move_begin:
    bge $t3, $t1, loop_closest_move_end
    
    # (move_x - player_x)^2
    lw $t4, ($t0)
    sub $t4, $t4, $s0
    mul $t4, $t4, $t4
    addi $t0, $t0, 4
      
    # (move_y - player_y)^2
    lw $t5, ($t0)
    sub $t5, $t5, $s1
    mul $t5, $t5, $t5
    addi $t0, $t0, 4
    
    # add
    add $t6, $t4, $t5
    
    # compare
    blt $t6, $t2, loop_closest_move_update_min
    
loop_closest_move_increment:
    addi $t3, $t3, 1 
    b loop_closest_move_begin
    
loop_closest_move_end:
    # outro
    lw $s1, 20($sp)
    lw $s0, 24($sp)
    lw $ra, 28($sp)
    addi $sp, $sp, 32
    jr $ra
    
loop_closest_move_update_min:
    move $t2, $t6
    move $v0 ,$t3
    b loop_closest_move_increment
   
#######################################################
# void check_save_dir(int x, int y)
# check if the direction is possible for a ghost to move, save the pos if it's available
check_save_dir:
    # intro 
    subi $sp, $sp, 32
    sw $ra, 28($sp) 
    sw $s0, 24($sp) # x
    sw $s1, 20($sp) # y
    move $s0, $a0
    move $s1, $a1

    # check pos
    jal is_pos_walkable_on_map
    beq $v0, $zero, check_save_dir_end
    
    # save pos
    lw $t1, ghost_available_moves_num
    sll $t0, $t1, 3 # one move contains 8 bytes data: 4 for x, 4 for y
    la $t2, ghost_available_moves
    add $t0, $t0, $t2
    
    sw $s0, ($t0)
    sw $s1, 4($t0)
    addi $t1, $t1, 1
    sw $t1, ghost_available_moves_num
    
check_save_dir_end:
    # outro
    lw $s1, 20($sp)
    lw $s0, 24($sp)
    lw $ra, 28($sp)
    addi $sp, $sp, 32
    jr $ra
    
#######################################################
# void calc_available_dir(int x, int y)
# find all possible directions for the position (x,y) and save them into ghost_available_moves_addr
calc_available_dir:
    # intro 
    subi $sp, $sp, 32
    sw $ra, 28($sp) 
    sw $s0, 24($sp) # x
    sw $s1, 20($sp) # y
    move $s0, $a0
    move $s1, $a1
    
    # up
    subi $a1, $a1, 1
    jal check_save_dir
    
    # down
    move $a0, $s0
    addi $a1, $s1, 1
    jal check_save_dir
    
    # left
    subi $a0, $s0, 1
    move $a1, $s1
    jal check_save_dir
    
    # right
    addi $a0, $s0, 1
    move $a1, $s1
    jal check_save_dir
  
   
    # outro
    lw $s1, 20($sp)
    lw $s0, 24($sp)
    lw $ra, 28($sp)
    addi $sp, $sp, 32
    jr $ra
    
#######################################################
# void draw_ghost(int x,int y, int color)
draw_ghost:
    # intro 
    subi $sp, $sp, 48
    sw $ra, 40($sp)
    sw $s4, 36($sp)
    sw $s3, 32($sp)
    sw $s2, 28($sp)
    sw $s1, 24($sp)
    sw $s0, 20($sp)
    sw $a2, 16($sp)
    
    # save args
    la $s1, ghost_bitmap
    lw $s0, ghost_bitmap_length
    
    li $s2, 0 # cur i
    lw $t0, pac_man_map_start_x
    add $a0, $t0, $a0
    jal tile_unit_to_pixel_pos
    move $s3, $v0
    
    lw $t0, pac_man_map_start_y
    add $a0, $a1, $t0
    jal tile_unit_to_pixel_pos
    move $s4, $v0
    
Ldraw_ghost_begin:
    # draw horizontal rectangles of (x, y, x_length, 1) using datasets from pac_man_player
    beq $s2, $s0,  Ldraw_ghost_end
    
    lb $a0, 0($s1)
    add $a0, $a0, $s3
    
    lb $a1, 1($s1)
    add $a1, $a1, $s4 
    
    lb $a2, 2($s1) 
    lb $a3, 3($s1)
    
    jal draw_rectangle
    
    addi $s2, $s2, 4
    addi $s1, $s1, 4
    b Ldraw_ghost_begin
    
Ldraw_ghost_end:
    # outro
    lw $ra, 40($sp)
    lw $s4, 36($sp)
    lw $s3, 32($sp)
    lw $s2, 28($sp)
    lw $s1, 24($sp)
    lw $s0, 20($sp)
    addi $sp, $sp, 48
    jr $ra
    
#######################################################
# void update_ghost_move_towards_player(int* x, int* y)
# update a ghost's position based on choosing a best move that will minimize the distance between the player's current position and the ghost's next position
update_ghost_move_towards_player:
    # intro 
    subi $sp, $sp, 32
    sw $ra, 28($sp)
    sw $s0, 24($sp)
    sw $s1, 20($sp)
    sw $s2, 16($sp)
    move  $s0, $a0
    move  $s1, $a1
    
    # calc availble
    lw $a0, ($a0)
    lw $a1, ($a1)    
    jal calc_available_dir
     
    # calc the best move
    jal get_move_towards_player
    move $s2, $v0
    
    # draw a black square at the previous location
    lw $a0, ($s0)
    lw $a1, ($s1)
    jal draw_a_black_square
    
    # redeem the result
    la $t0, ghost_available_moves
    sll $s2, $s2, 3 # # one move contains 8 bytes data: 4 for x, 4 for y
    add $s2, $s2, $t0
    lw $t0, ($s2)
    addi $s2, $s2, 4
    lw $t1, ($s2)
   
    # save 
    sw $t0, ($s0)
    sw $t1, ($s1)
    
    # reset
    sw $zero, ghost_available_moves_num

    # outro
    lw $s2, 16($sp)
    lw $s1, 20($sp)
    lw $s0, 24($sp)
    lw $ra, 28($sp)
    addi $sp, $sp, 32
    jr $ra
    
        

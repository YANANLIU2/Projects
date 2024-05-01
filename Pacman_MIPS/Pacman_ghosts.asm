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

blinky_pos_x: .word 11
blinky_pos_y: .word 9

pinky_pos_x: .word 11
pinky_pos_y: .word 11

inky_pos_x: .word 10
inky_pos_y: .word 11

clyde_pos_x: .word 12
clyde_pos_y: .word 11

error_msg_out_of_range: .asciiz "Erros: Random number is out of range.\n"

.text

#######################################################
.globl draw_all_ghosts
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
    jal update_ghost_random_dir
    
    # update "Pinky (pink)"
    la $a0, pinky_pos_x
    la $a1, pinky_pos_y
    jal update_ghost_random_dir
    
    # update "Inky (cyan)"
    la $a0, inky_pos_x
    la $a1, inky_pos_y
    jal update_ghost_random_dir
    
    # update "Clyde (orange)"
    la $a0, clyde_pos_x
    la $a1, clyde_pos_y
    jal update_ghost_random_dir
    
    # outro
    lw $ra, 20($sp)
    addi $sp, $sp, 24
    jr $ra
    
#######################################################
# vood update_ghost_random_dir(int* x, int* y)
# update a ghost's dir based on choosing an available direction randomly
update_ghost_random_dir:
    # intro 
    subi $sp, $sp, 40
    sw $ra, 36($sp)
    sw $s0, 32($sp)
    sw $s1, 28($sp)
    sw $s2, 24($sp)
    sw $s3, 20($sp)
 
    # load values
    lw $s0, ($a0) # x
    lw $s1, ($a1) # y
    move $s2, $a0 # int* x
    move $s3, $a1 # int* y
    
    # overwrite the previous pacman position with a black square
    move $a0, $s0
    move $a1, $s1 
    jal draw_a_black_square
    
    # get a randdom available direction
    move $a0, $s0
    move $a1, $s1
    jal get_rand_available_dir
     
    beq $v0, $zero, Lmove_up
    beq $v0, 1, Lmove_down
    beq $v0, 2, Lmove_left
    beq $v0, 3, Lmove_right
    b Lerror_msg
    
Lmove_up:
    subi $s1, $s1, 1
    sw $s1, ($s3)
    b Lupdate_ghost_end
    
Lmove_down:
    addi $s1, $s1, 1
    sw $s1, ($s3)
    b Lupdate_ghost_end

Lmove_left:
    subi $s0, $s0, 1
    sw $s0, ($s2)
    b Lupdate_ghost_end
    
Lmove_right:
    addi $s0, $s0, 1
    sw $s0, ($s2)
    b Lupdate_ghost_end
    
Lerror_msg:
    la $a0, error_msg_out_of_range
    li $v0, 4
    syscall

Lupdate_ghost_end:
    lw $ra, 36($sp)
    lw $s0, 32($sp)
    lw $s1, 28($sp)
    lw $s2, 24($sp)
    lw $s3, 20($sp)
    addi $sp, $sp, 40
    jr $ra

#######################################################
# int get_rand_available_dir(int x, int y)
# find all possible directions for the position (x,y)
# choose a random one from the available ones
# return: 0 (up), 1 (down), 2 (left), 3 (right)
# ideally I want to use heap memory here, but I used heap's address for display. So I use stack for my local array. 
get_rand_available_dir:
    # intro 
    subi $sp, $sp, 40
    sw $ra, 36($sp)
    sw $zero, 32($sp) # size of the available direction array
    sw $a0, 40($sp) # previous stack frame
    sw $a1, 44($sp)
    
 # Lcalc_up:
    subi $a1, $a1, 1
    jal is_pos_walkable_on_map 
    beq $v0, 0, Lcalc_down # 0 means not walkable
    
    li $t0, 1
    sw $t0, 32($sp) # update array size
    sw $zero, 28($sp) # save up(0) into array
    
 Lcalc_down:
    lw $a0, 40($sp)
    lw $a1, 44($sp)
    addi $a1, $a1, 1
    jal is_pos_walkable_on_map
    beq $v0, 0, Lcalc_left
    
    lw $t0, 32($sp)
    addi $t0, $t0, 1
    sw $t0, 32($sp) # update array size
    
    sll $t0, $t0, 2
    la $t1,  32($sp)
    sub $t0, $t1, $t0 # calc addr
    
    li $t1, 1
    sw $t1, ($t0) # save down(1) into array

Lcalc_left:
    lw $a0, 40($sp)
    lw $a1, 44($sp)
    subi $a0, $a0, 1
    jal is_pos_walkable_on_map 
    beq $v0, 0, Lcalc_right
    
    lw $t0, 32($sp)
    addi $t0, $t0, 1
    sw $t0, 32($sp) # update array size
    
    sll $t0, $t0, 2
    la $t1,  32($sp)
    sub $t0, $t1, $t0 # calc addr
    
    li $t1, 2
    sw $t1, ($t0) # save left(2) into array

Lcalc_right:
    lw $a0, 40($sp)
    lw $a1, 44($sp)
    addi $a0, $a0, 1
    jal is_pos_walkable_on_map 
    beq $v0, 0, Lcalc_rand
    
    lw $t0, 32($sp)
    addi $t0, $t0, 1
    sw $t0, 32($sp) # update array size
    
    sll $t2, $t0, 2
    la $t1,  32($sp)
    sub $t2, $t1, $t2 # calc addr
    
    li $t1, 3
    sw $t1, ($t2) # save right(3) into array
    
Lcalc_rand:
    li $v0, 0 # default return value
    lw $t0, 32($sp)
    ble $t0, $zero, Lcalc_exit
    
    # generate a rand dir 
    move $a1, $t0
    li $v0, 42  
    syscall
    
    # get return value from array
    sll $a0, $a0, 2
    la $t1,  28($sp)  # array[0]
    sub $a0, $t1, $a0 # calc addr
    lw $v0, ($a0)

Lcalc_exit:
    # outro
    lw $ra, 36($sp) 
    addi $sp, $sp, 40
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
        

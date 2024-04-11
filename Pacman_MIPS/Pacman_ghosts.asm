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
blinky_direction_x: .word 0
blinky_direction_y: .word 0

pinky_pos_x: .word 11
pinky_pos_y: .word 11
pinky_direction_x: .word 0
pinky_direction_y: .word 0

inky_pos_x: .word 10
inky_pos_y: .word 11
inky_direction_x: .word 0
inky_direction_y: .word 0

clyde_pos_x: .word 12
clyde_pos_y: .word 11
clyde_direction_x: .word 0
clyde_direction_y: .word 0

error_msg_out_of_range: .asciiz "Erros: Random number is out of range."

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
    jal update_ghost_random
    
    # update "Pinky (pink)"
    
    # update "Inky (cyan)"
    
    # update "Clyde (orange)"
    
    # outro
    lw $ra, 20($sp)
    addi $sp, $sp, 24
    jr $ra
    
#######################################################
# vood update_ghost_random(int* x, int* y)
# update a ghost's pos based on choosing an available movement randomly
update_ghost_random:
    # save args
    move $t0, $a0
    move $t1, $a1
    
    # generate a rand dir 
    li $a1, 4
    li $v0, 42  
    syscall
    
    # load values
    lw $t2, ($t0)
    lw $t3, ($t1)
    
    beq $a0, $zero, Lmove_up
    beq $a0, 1, Lmove_down
    beq $a0, 2, Lmove_left
    beq $a0, 3, Lmove_right
    b Lerror_msg
    
Lmove_up:
    subi $t3, $t3, 1
    sw $t3, ($t1)
    b Lupdate_ghost_end
    
Lmove_down:
    addi $t3, $t3, 1
    sw $t3, ($t1)
    b Lupdate_ghost_end

Lmove_left:
    subi $t2, $t2, 1
    sw $t2, ($t0)
    b Lupdate_ghost_end
Lmove_right:
    addi $t2, $t2, 1
    sw $t2, ($t0)
    b Lupdate_ghost_end
    
Lerror_msg:
    la $a0, error_msg_out_of_range
    li $v0, 4
    syscall

Lupdate_ghost_end:
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
        

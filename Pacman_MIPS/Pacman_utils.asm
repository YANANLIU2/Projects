.data
# Base address for the bitmap display
base_address: .word 0x10040000
display_width_units: .word 512 # display width/ unit width in pixels
display_tile_scale: .word 4 # in the form of 2**n 
tile_size_pixels: .word 16 # one tile = 16 pixels
color_blue: .word 0x000000FF
color_gray: .word 0x00EEEEEE
color_yellow: .word 0x00FFFF00
color_black: .word 0x00000000

.text

.globl  draw_rectangle
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
    
.globl tile_unit_to_pixel_pos
#######################################################
# int tile_unit_to_pixel_pos(int unit_pos)
# translate a tile pos to a pixel pos
tile_unit_to_pixel_pos:
    lw $t0, display_tile_scale
    sllv $v0, $a0, $t0
    jr $ra 
    
.globl draw_a_black_square
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
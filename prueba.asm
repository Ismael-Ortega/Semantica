;Archivo: prueba.asm
;Fecha: 10/11/2022 06:49:03 p. m.
#make_COM#
include 'emu8086.inc'
ORG 100H
;Variables: 
	area  DW ?
	radio  DW ?
	pi  DW ?
	resultado  DW ?
	a  DW ?
	d  DW ?
	altura  DW ?
	cinco  DW ?
	x  DW ?
	y  DW ?
	i  DW ?
	j  DW ?
	k  DW ?
	ab  DW ?
MOV AX, i
PUSH AX
MOV AX, 3
PUSH AX
POP BX
POP AX
CMP AX, BX
JGE 
MOV AX, i
PUSH AX
POP AX
CALL PRINT_NUM
RET
DEFINE_PRINT_NUM
DEFINE_PRINT_NUM_UNS
DEFINE_SCAN_NUM
END

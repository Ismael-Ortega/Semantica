;Archivo: prueba.asm
;Fecha: 09/11/2022 08:23:21 a. m.
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
MOV AH, 0
MOV AX, 257
PUSH AX
POP AX
POP AX
MOV ab, AX
POP AX
RET
DEFINE_SCAN_NUM
END

;Archivo: prueba.asm
;Fecha: 07/11/2022 01:46:25 p. m.
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
MOV AX, 200
PUSH AX
POP AX
MOV ab, AX
MOV AX, 2
PUSH AX

;Archivo: prueba.asm
;Fecha: 26/10/2022 10:03:48 a. m.
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
	k  DW ?
	l  DW ?
	x  DW ?
	y  DW ?
	i  DW ?
	j  DW ?
inicioFor0:
MOV AX, 0
PUSH AX
POP AX
MOV i, AX
MOV AX, 1
PUSH AX
POP AX
POP BX
CMP AX, BX
JGE 

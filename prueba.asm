;Archivo: prueba.asm
;Fecha: 07/11/2022 09:58:04 a. m.
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
PRINTN "Introduce la altura de la piramide: "
CALL SCAN_NUM
MOV altura, CX
MOV AX, 2
PUSH AX
POP AX
POP BX
CMP AX, BX
JLE if1
inicioFor0:
POP AX
MOV i, AX
MOV AX, 0
PUSH AX
POP AX
POP BX
CMP AX, BX
JLE 

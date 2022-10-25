;Archivo: prueba.asm
;Fecha: 24/10/2022 09:43:50 a. m.
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
	x  DW ?
	y  DW ?
	i  DW ?
	j  DW ?
MOV AX, 61
PUSH AX
POP AX
MOV y, AX
MOV AX, 61
PUSH AX
POP AX
POP BX
MOV AX, 0
PUSH AX
POP AX
MOV x, AX
MOV AX, 5
PUSH AX
POP AX
POP BX
MOV AX, 0
PUSH AX
POP AX
if2:
MOV AX, 10
PUSH AX
POP AX
MOV x, AX
if1:
RET
END

;Archivo: prueba.asm
;Fecha: 08/11/2022 12:50:29 p. m.
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
MOV AX, 0
PUSH AX
POP AX
MOV a, AX
MOV AX, 1
PUSH AX
POP AX
POP BX
CMP AX, BX
JNE if1
PRINTN "Hola"
if1:
PRINTN "Adios"
if1:
RET
DEFINE_SCAN_NUM
END

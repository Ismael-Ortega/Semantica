;Archivo: prueba.asm
;Fecha: 10/11/2022 08:59:54 a. m.
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
MOV AX, 1
PUSH AX
MOV AX, 1
PUSH AX
POP AX
POP BX
CMP AX, BX
JNE if1
PRINTN "Hola"
JMP else1
if1:
PRINTN "Adios"
else1:
RET
DEFINE_PRINT_NUM
DEFINE_PRINT_NUM_UNS
DEFINE_SCAN_NUM
END

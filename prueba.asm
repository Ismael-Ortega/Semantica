;Archivo: prueba.asm
;Fecha: 28/10/2022 09:36:41 a. m.
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
PRINTN "Introduzca el radio del cilindro: "
CALL SCAN_NUM
MOV radio, CX
RET
DEFINE_SCAN_NUM

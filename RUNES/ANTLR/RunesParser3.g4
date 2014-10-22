parser grammar RunesParser;

options {
	tokenVocab=RunesLexer;
}

effect		: statlist;

statlist	: stat (NEWLINE|SEMICOLON) stat;

stat		: WHEN player condition LBRACE statlist RBRACE
			| player action;

player		: USER
			| ENEMY
			;

action		: cardaction NUMBER CARDS?
			| hpaction NUMBER DAMAGE?
			;

cardaction	: DRAWS
			| DISCARDS
			;

hpaction	: TAKES
			| HEALS
			| SHIELDS
			;

condition	: cardaction inequality NUMBER CARDS?
			| hpaction inequality NUMBER DAMAGE?
			;

inequality	: GT
			| LT
			| GTEQ
			| LTEQ
			| EQ
			| NEQ
			;
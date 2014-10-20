parser grammar RunesParser;

options {
	tokenVocab=RunesLexer;
}

effect		: (preCond)? stat;

preCond		: ACTIVATE option WHEN condition;

option		: OPTION | AUTO ;

// TODO(ticktakashi): Rethink the grammar to allow for repeating the #
// same actions at one time (e.g. Magic Missle)
// and also allow for checking a condition multiple times
// (e.g. the next 3 times your opponent does blah)
stat		: value LBRACE stat RBRACE				# statRepeat
			| WHEN condition LBRACE stat RBRACE		# statWhen
			| player scalarEffect value				# statScalar
			| player cardEffect value card			# statCard
			| stat stat								# statList
			;

			// In the future, add a (WITH card)? extension to these scalar
condition	: player scalarEffect ineq value		# condScalar
			| player cardEffect card ineq value		# condCard
			| condition binopBool condition			# condExpr
			| NOT condition							# condNot
			| LPAREN condition RPAREN				# condParen
			;

player		: USER | ENEMY ;

scalarEffect: DRAWS | TAKES | HEALS | SHIELDS | PIERCES;

cardEffect	: DISCARDS | FINDS;

card		: ALL | MELEE | SKILLSHOT | TARGETED | MOBILITY | PASSIVE | NUMBER | THIS  ;

value		: NUMBER ; // In future, this might be a value from the card like shield etc.

binopBool	: OR | AND ;

ineq		: GT | LT | EQ ;
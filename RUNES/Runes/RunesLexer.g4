lexer grammar RunesLexer;

WS : [' \n\t'] -> channel(HIDDEN);

// Conditionals
WHEN		: 'WHEN';
CHARGES : 'CHARGES';
TIMES   : 'TIMES';

// Targets
USER		: 'USER';
ENEMY		: 'ENEMY';
ANY			: 'ANY';

// Card Types
ALL			: 'ALL';
SKILLSHOT	: 'SKILLSHOT';
TARGETED	: 'TARGETED';
MOBILITY	: 'MOBILITY';
MELEE		: 'MELEE';
PASSIVE		: 'PASSIVE';
THIS		: 'THIS';
FINDS		: 'FINDS';

// General Punctuation
LBRACE		: '{';
RBRACE		: '}';
LPAREN		: '(';
RPAREN		: ')';
SEMICOLON	: ';';

// Activation 
ACTIVATE	: 'ACTIVATE';
OPTION		: 'OPTIONALLY' | 'OPTION';
AUTO		: 'AUTOMATICALLY' | 'AUTO';

// Number Based Commands
DRAWS		: 'DRAWS';
TAKES		: 'TAKES';
DISCARDS	: 'DISCARDS';
HEALS		: 'HEALS';
SHIELDS		: 'SHIELDS';
PIERCES		: 'PIERCES';
//...

// Card Based Commands
PLAYS		: 'PLAYS';
// ...

// Equality operators
GT			: '>=';
LT			: '<=';
EQ			: '==';

// Boolean operators
AND			: 'AND' | '&&';
OR			: 'OR'	| '||';
NOT			: 'NOT' | '!';

NUMBER		: [0-9];

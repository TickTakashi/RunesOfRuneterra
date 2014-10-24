lexer grammar CARDScriptLexer;

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
ALL			  : 'ALL';
SKILLSHOT	: 'SKILLSHOT';
TARGETED	: 'TARGETED';
MOBILITY	: 'MOBILITY';
MELEE		  : 'MELEE';
PASSIVE		: 'PASSIVE';
THIS		  : 'THIS';

// Locations
HAND      : 'HAND';
DECK      : 'DECK';
COOLDOWN  : 'COOLDOWN';

// Card Properties
SHIELD    : 'SHIELD';
DAMAGE    : 'DAMAGE';
DURATION  : 'DURATION';

// General Punctuation
LBRACE		: '{';
RBRACE		: '}';
LPAREN		: '(';
RPAREN		: ')';
SEMICOLON	: ';';
TILDE     : '~';
FROM      : 'FROM';
TO        : 'TO';
DOT       : '.';
IN        : 'IN';

// Activation 
ACTIVATE	: 'ACTIVATE';
OPTION		: 'OPTIONALLY' | 'OPTION';
AUTO		  : 'AUTOMATICALLY' | 'AUTO';

// Scalar actions
DRAWS		  : 'DRAWS';
TAKES		  : 'TAKES';
DISCARDS	: 'DISCARDS';
HEALS		  : 'HEALS';
SHIELDS		: 'SHIELDS';
PIERCES		: 'PIERCES';
//...

// Card actions
PLAYS		: 'PLAYS';
ADDS    : 'ADDS';
// ...

// Equality operators
GT			: '>=';
LT			: '<=';
EQ			: '==';

// Boolean operators
AND			: 'AND' | '&&';
OR			: 'OR'	| '||';
NOT			: 'NOT' | '!';

// Vaule modifiers 
HALF    : 'HALF';
DOUBLE  : 'DOUBLE';

// Values and Numbers
IDENT   : [_a-zA-Z]+ [_a-zA-Z0-9]*;
NUMBER	: [0-9]+;

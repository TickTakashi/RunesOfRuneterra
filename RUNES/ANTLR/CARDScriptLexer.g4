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
ALL			: 'ALL';
SKILL  	: 'SKILL';
SPELL 	: 'SPELL';
DASH    : 'DASH';
MELEE		: 'MELEE';
SELF    : 'SELF';
PASSIVE : 'PASSIVE';
THIS		: 'THIS';

// Locations
HAND    : 'HAND';
DECK    : 'DECK';
COOL    : 'COOLDOWN';

// Card Properties
DAMAGE  : 'DAMAGE';
RANGE   : 'RANGE';
COST    : 'COST';
LIMIT   : 'LIMIT';
EFFECT  : 'EFFECT';

// CC Effects
CHANNEL : 'CHANNEL';
SLOW    : 'SLOW';
SNARE   : 'SNARE';
STUN    : 'STUN';
KNOCK   : 'KNOCK';
SILENCE : 'SILENCE';
BLIND   : 'BLIND';

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

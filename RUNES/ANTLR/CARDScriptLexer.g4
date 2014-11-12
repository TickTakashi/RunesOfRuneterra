lexer grammar CARDScriptLexer;

NAME : DQUOTE CHARACTER* DQUOTE ;
fragment DQUOTE : '\"';
fragment CHARACTER :  ~['\"'] | WS ;

COMMENT : ('#' ~[\n]*) -> skip;
WS      : [ \t\n\r]+ -> skip;

// Conditionals
WHEN		: 'WHEN';
IF      : 'IF';
ELSE    : 'ELSE';
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
TIME    : 'TIME';

// CC Effects
CHANNEL : 'CHANNEL';
SLOW    : 'SLOW';
SNARE   : 'SNARE';
STUN    : 'STUN';
KNOCKBACK   : 'KNOCKBACK';
KNOCKUP     : 'KNOCKUP';
SILENCE : 'SILENCE';
BLIND   : 'BLIND';

// State Properties
HEALTH  : 'HEALTH';

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
SHIELD		: 'SHIELD';
MELEE_RANGE   : 'MELEE_RANGE';
MELEE_DAMAGE  : 'MELEE_DAMAGE';
SKILL_DAMAGE  : 'SKILL_DAMAGE';
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
E       : '=';

// Boolean operators
AND			: 'AND' | '&&';
OR			: 'OR'	| '||';
NOT			: 'NOT' | '!';

// Value modifiers 
HALF    : 'HALF';
DOUBLE  : 'DOUBLE';
DISTANCE: 'DISTANCE';

// Values and Numbers
IDENT   : [_a-zA-Z]+ [_a-zA-Z0-9]*;
NUM	    : [0-9]+;

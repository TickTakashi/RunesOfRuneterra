lexer grammar CARDScriptLexer;

NAME : DQUOTE CHARACTER* DQUOTE ;
fragment DQUOTE : '\"';
fragment CHARACTER :  ~['\"'] | WS ;

COMMENT : ('#' ~[\n]*) -> skip;
WS      : [ \t\n\r]+ -> skip;




// Conditionals
IF      : 'IF';
ELSE    : 'ELSE';

// Targets
USER		: 'USER';
ENEMY		: 'ENEMY';

// Card Types
SKILL  	: 'SKILL';
SPELL 	: 'SPELL';
MELEE		: 'MELEE';
SELF    : 'SELF';

// Locations
HAND    : 'HAND';
DECK    : 'DECK';
COOL    : 'COOL';

// Card Properties
DAMAGE  : 'DAMAGE';
RANGE   : 'RANGE';
COST    : 'COST';
TIME    : 'TIME';
DASH    : 'DASH';
LIMIT   : 'LIMIT';
EFFECT  : 'EFFECT';
BUFF    : 'BUFF';
ULTIMATE: 'ULTIMATE';
ID      : 'ID';
TYPE    : 'TYPE';
TITLE   : 'NAME' | 'TITLE';

// CC Effects
SLOW    : 'SLOW';
SNARE   : 'SNARE';
STUN    : 'STUN';
SILENCE : 'SILENCE';
BLIND   : 'BLIND';

// Action Effects
KNOCKBACK : 'KNOCKBACK';
KNOCKUP   : 'KNOCKUP';
SHIELD    : 'SHIELD';

// Normal Activation
ACTIVATE  : 'ACTIVATE';
NORMAL    : 'NORMAL';
AS        : 'AS';

// General Punctuation
LBRACE		: '{';
RBRACE		: '}';
LPAREN		: '(';
RPAREN		: ')';
TILDE     : '~';
FROM      : 'FROM';
TO        : 'TO';
IN        : 'IN';
MAY       : 'MAY';
TIMES     : 'TIMES';

// Scalar actions
DRAWS		  : 'DRAWS';
TAKES		  : 'TAKES';
DISCARDS	: 'DISCARDS';
HEALS		  : 'HEALS';

// Flat Buff Types
MELEE_R   : 'MELEE_R';
MELEE_D   : 'MELEE_D';
SKILL_D   : 'SKILL_D';
SPELL_D   : 'SPELL_D';

// Card actions
ADDS    : 'ADDS';

// Equality operators
GT			: '>';
GTE     : '<=';
LT			: '<';
LTE     : '<=';
EQ			: '==';
NEQ     : '!=';
E       : '=';

// Boolean operators
AND			: 'AND' | '&&';
OR			: 'OR'	| '||';
NOT			: 'NOT' | '!';

// Value
HALF    : 'HALF';
DOUBLE  : 'DOUBLE';
DISTANCE: 'DISTANCE';
HEALTH  : 'HEALTH';

// Values and Numbers
IDENT   : [_a-zA-Z]+ [_a-zA-Z0-9]*;
NUM	    : [0-9]+;

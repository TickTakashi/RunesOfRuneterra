parser grammar CARDScriptParser;

options {
	tokenVocab=CARDScriptLexer;
}

cardDesc    : SKILL       cardStats
            | SPELL       cardStats
            | MELEE       cardStats
            | DASH NUMBER cardStats
            | SELF        selfStats
            | PASSIVE     effect 
            ;

cardStats   : DAMAGE EQ NUMBER 
              RANGE EQ NUMBER
              COST EQ NUMBER
              LIMIT EQ NUMBER
              effect ; 

effect		  : (preCond)? stat;

preCond		  : ACTIVATE option WHEN condition;

option		  : OPTION | AUTO ;

// TODO(ticktakashi): Rethink the grammar to allow for repeating the #
// same actions at one time (e.g. Magic Missle)
// and also allow for checking a condition multiple times
// (e.g. the next 3 times your opponent does blah)
stat		    : when                              # statWhen
            | action                            # statAction
			      | action value TIMES				        # actionRepeat
			      | stat stat								          # statList
			      ;

when        : WHEN condition LBRACE stat RBRACE (value CHARGES)? ;

action      : player scalarEffect value         # actionScalar
            | player cardEffect value cardTarget      # actionCard
            ;

// In the future, add a (WITH card)? extension to these scalar to check
// if, for example, they did 2 damage with a MELEE attack
condition	  : player scalarEffect ineq value		# condScalar
			      | player cardEffect cardTarget ineq value	# condCard
			      | condition binopBool condition			# condExpr
			      | NOT condition							        # condNot
			      | LPAREN condition RPAREN				    # condParen
			      ;

player		  : USER | ENEMY ;

scalarEffect: DRAWS | TAKES | HEALS | SHIELDS | PIERCES;

cardEffect	: DISCARDS | FINDS;

cardTarget  : ALL | MELEE | SKILLSHOT | TARGETED | MOBILITY | PASSIVE 
            | NUMBER | THIS ;

// In future, this might be a value from the card like shield etc.
// TODO(ticktakashi): Add Card Value.
value		    : NUMBER            # valueInt
            | value TILDE value # valueRandom
            ;

binopBool	  : OR | AND  ;

ineq		    : GT | LT | EQ ;
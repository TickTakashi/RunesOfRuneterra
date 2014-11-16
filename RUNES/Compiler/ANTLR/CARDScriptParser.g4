parser grammar CARDScriptParser;

options {
	tokenVocab=CARDScriptLexer;
}

passiveDesc : NAME E? NUM cardB ;

cardDesc    : cardID SKILL DAMAGE E? NUM RANGE E? NUM cardE # cardSkill
            | cardID SPELL DAMAGE E? NUM RANGE E? NUM cardE # cardSpell
            | cardID MELEE DAMAGE E? NUM cardE              # cardMelee
            | cardID SELF  TIME   E? NUM cardE cardB        # cardSelf
            ;

cardID      : NAME E? NUM COST E? NUM LIMIT E? NUM (DASH E? NUM)? ULTIMATE? ;

cardE       : EFFECT E? effect ;

cardB       : BUFF E? buffEffect ;

effect		  : LBRACE statE? RBRACE ;

buffEffect  : LBRACE statB? RBRACE ;

statB       : player buff value                   # statBFlat
            ;

statE		    : NORMAL ACTIVATION AS cardType       # statENormal
            | IF stateCond effect (ELSE effect)?  # statEIf
            | action value TIMES                  # statEAction
			      | statE statE							            # statEList
			      ;

stateCond   : value ineq value                    # stateCondHealth
            | stateCond binopBool stateCond       # stateCondBinop
            | NOT stateCond                       # stateCondNot
            | LPAREN stateCond RPAREN             # stateCondParen
            ;

action      : KNOCKBACK NUM                       # actionKnockback
            | KNOCKUP NUM                         # actionKnockup
            | SHIELD NUM                          # actionShield
            | ccEffect NUM                        # actionCC
            | player scalarE value                # actionScalar  
            | player ADDS value NAME FROM 
              location TO location                # actionSearch  
            ;

value		    : NUM                                 # valueInt
            | value TILDE value                   # valueRandom
            | HALF value                          # valueHalf  
            | DOUBLE value                        # valueDouble 
            | DISTANCE                            # valueDistance 
            | player HEALTH                       # valueHealth
            | cardTarget IN player location       # valueCardCount  
            ;

player		  : USER | ENEMY ;
location    : HAND | DECK | COOL ;
scalarE     : DRAWS | TAKES | HEALS;
buff        : MELEE_D | MELEE_R | SKILL_D ;
ccEffect	  : SLOW | SNARE | STUN | KNOCKUP | SILENCE | BLIND ;
binopBool	  : OR | AND  ;
ineq		    : GT | LT | EQ ;
cardType    : SKILL | SPELL | MELEE | SELF ;
cardTarget	: NAME | NUM | THIS ;

/*
// TODO(ticktakashi): Add a (WITH card)? extension to these scalar to check if, for example, they did 2 damage with a MELEE attack
// TODO(ticktakashi): Add variables here. like binding card to a identifier x in cond Play
// TODO(ticktakashi): Think about adding whenConds for card drawing and cooldown etc. (Seems obscure, add as necessary).
eventCond	  : player PLAYS cardTarget (IDENT)?  # eventCondPlay    // TODO(ticktakashi): Implement IDENT component of this.
            | player scalarEffect ineq value		# eventCondScalar
			      | eventCond binopBool eventCond     # eventCondExpr
			      | NOT eventCond							        # eventCondNot
			      | LPAREN eventCond RPAREN				    # eventCondParen
			      ;

cardProperty: DAMAGE | RANGE | COST;
*/
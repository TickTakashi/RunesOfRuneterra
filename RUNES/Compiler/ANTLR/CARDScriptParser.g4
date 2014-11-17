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

cardE       : EFFECT E? effect ;                                    // TODO

cardB       : BUFF E? buffEffect ;                                  // TODO

effect		  : LBRACE statE? RBRACE ;                                // TODO

buffEffect  : LBRACE statB? RBRACE ;                                // TODO

statB       : player buff value                   # statBFlat       // TODO
            ;

statE		    : NORMAL ACTIVATION AS cardType       # statENormal
            | action                              # statEAction     // TODO
            | IF stateCond effect (ELSE effect)?  # statEIf         // TODO
			      | statE statE							            # statEList       // TODO
            | statE value TIMES                   # statERepeat     // TODO
			      ;

stateCond   : value ineq value                    # stateCondHealth // TODO
            | stateCond binopBool stateCond       # stateCondBinop  // TODO
            | NOT stateCond                       # stateCondNot    // TODO
            | LPAREN stateCond RPAREN             # stateCondParen  // TODO
            ;

action      : SHIELD NUM                          # actionShield
            | KNOCKUP NUM                         # actionKnockup
            | KNOCKBACK NUM                       # actionKnockback
            | ccEffect NUM                        # actionCC
            | player scalarE value                # actionScalar    // TODO
            | player ADDS value NAME FROM 
              location TO location                # actionSearch    // TODO
            ;

value		    : NUM                                 # valueInt        // TODO
            | value TILDE value                   # valueRandom     // TODO
            | HALF value                          # valueHalf       // TODO
            | DOUBLE value                        # valueDouble     // TODO
            | DISTANCE                            # valueDistance   // TODO
            | player HEALTH                       # valueHealth     // TODO
            | cardTarget IN player location       # valueCardCount  // TODO
            ;

player		  : USER | ENEMY ;
location    : HAND | DECK | COOL ;
scalarE     : DRAWS | TAKES | HEALS;
buff        : MELEE_D | MELEE_R | SKILL_D ;
ccEffect	  : SLOW | SNARE | STUN | SILENCE | BLIND ;
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
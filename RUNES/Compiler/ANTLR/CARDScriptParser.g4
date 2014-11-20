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

statB       : bonusB value                        # statBFlat       
            ;

statE		    : ACTIVATE AS cardType                # statENormal
            | action                              # statEAction
            | IF stateCond effect (ELSE effect)?  # statEIf
			      | statE statE							            # statEList
            | statE value TIMES                   # statERepeat     // TODO
			      ;

stateCond   : value ineq value                    # stateCondIneq
            | stateCond binopBool stateCond       # stateCondBinop  // TODO
            | NOT stateCond                       # stateCondNot    // TODO
            | LPAREN stateCond RPAREN             # stateCondParen  // TODO
            ;

action      : SHIELD NUM                          # actionShield
            | KNOCKUP NUM                         # actionKnockup
            | KNOCKBACK NUM                       # actionKnockback
            | ccEffect NUM                        # actionCC
            | player scalarE value                # actionScalar
            | player (MAY)? ADDS value cardCond 
              FROM player location TO player 
              location                            # actionSearch    
            ;

value		    : NUM                                 # valueInt
            | value TILDE value                   # valueRandom
            | HALF value                          # valueHalf 
            | DOUBLE value                        # valueDouble
            | DISTANCE                            # valueDistance
            | player HEALTH                       # valueHealth
            | cardCond IN player location         # valueCardCount
            ;

cardCond    : TITLE E NAME                        # cardCondName
            | TYPE E cardType                     # cardCondType
            | DASH                                # cardCondDash
            | ULTIMATE                            # cardCondUlt
            | cardP ineq value                    # cardCondcardP   // TODO
            | cardCond binopBool cardCond         # cardCondBinop   // TODO
            | NOT cardCond                        # cardCondNot     // TODO
            | LPAREN cardCond RPAREN              # cardCondParen   // TODO
            ;
                                   
player		  : USER | ENEMY ;
location    : HAND | DECK | COOL ;
scalarE     : DRAWS | TAKES | HEALS ;
bonusB      : MELEE_D | MELEE_R | SKILL_D | SPELL_D ;
cardP       : DAMAGE | RANGE | TIME ;
ccEffect	  : SLOW | SNARE | STUN | SILENCE | BLIND ;
binopBool	  : OR | AND ;
ineq		    : GT | GTE | LT | LTE | EQ | NEQ ;
cardType    : SKILL | SPELL | MELEE | SELF | DAMAGE ;
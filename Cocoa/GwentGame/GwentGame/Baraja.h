//
//  Baraja.h
//  GwentGame
//
//  Created by Adrian on 27/10/2019.
//  Copyright © 2019 Adrian A. G. Pazos. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Card.h"
NS_ASSUME_NONNULL_BEGIN

@interface Baraja : NSObject{
    NSMutableArray * cartas;
}

@property NSMutableArray *cartas;

-(void)darVueltaCarta:(Card *) card;
-(int)numCartasEnLaBaraja;
-(void)addCard:(Card *)card;
-(void)deleteCard:(Card *)card;
-(Card *)cardWithIndex:(NSInteger)i;

@end

NS_ASSUME_NONNULL_END
